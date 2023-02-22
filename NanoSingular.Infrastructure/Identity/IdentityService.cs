using NanoSingular.Application.Common;
using NanoSingular.Application.Common.Images;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Infrastructure.Identity.DTOs;
using NanoSingular.Infrastructure.Mailer;
using NanoSingular.Infrastructure.Utility;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


// Service for profile and user management
namespace NanoSingular.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentTenantUserService _currentTenantUserService;
        private readonly IMailService _mailService;
        private readonly IImageService _imageService;

        public IdentityService(IMapper mapper, UserManager<ApplicationUser> userManager, ICurrentTenantUserService currentTenantUserService, IMailService mailService, IImageService imageService)
        {
            _mapper = mapper; // Automapper
            _userManager = userManager; // Asp Identity user manager 
            _currentTenantUserService = currentTenantUserService; // current tenant/user (set by middleware)
            _mailService = mailService; // For password reset
            _imageService = imageService; // For profile image upload
        }

        // Get Users (Admin) - Full List
        // --- For Client-Side pagination (reactTable - users, tenants)  
        public async Task<Response<IEnumerable<UserDto>>> GetUsersAsync()
        {

            var usersList = await _userManager.Users.OrderByDescending(x => x.CreatedOn).ToListAsync();


            foreach (var user in usersList)
            {
                var roles = _userManager.GetRolesAsync(user);
                var roleId = roles.Result.FirstOrDefault();
                user.RoleId = roleId; // RoleId is a non-mapped property in the applicationUser class, the DTOs returned will have role ID
            }

            var userDtoList = new List<UserDto>();
            var dtoList = _mapper.Map(usersList, userDtoList);


            return Response<IEnumerable<UserDto>>.Success(dtoList);
        }

        // Get Users (Admin) - Paginated / Filtered List
        // --- For Server-Side pagination (serverTable - venues)
        public async Task<PaginatedResponse<UserDto>> GetUsersPaginatedAsync(UserListFilter filter)
        {

            var usersList = await _userManager.Users.ToListAsync();


            foreach (var user in usersList)
            {
                var roles = _userManager.GetRolesAsync(user);
                var roleId = roles.Result.FirstOrDefault();
                user.RoleId = roleId;
            }

            var userDtoList = new List<UserDto>();
            var dtoList = _mapper.Map(usersList, userDtoList);


            // Search keyword expression
            List<UserDto> filteredList;
            if (!String.IsNullOrWhiteSpace(filter.Keyword))
            {
                filteredList = dtoList.Where(x => x.FirstName.ToLower().Contains(filter.Keyword.ToLower()) || x.LastName.ToLower().Contains(filter.Keyword.ToLower())).ToList();
            }
            else
            {
                filteredList = dtoList;
            }


            // Paginate
            var totalCount = filteredList.Count();

            var pagedData = filteredList
             .Skip(((filter.PageNumber - 1) * filter.PageSize))
              .Take(filter.PageSize).ToList();


            return new PaginatedResponse<UserDto>(pagedData, totalCount, filter.PageNumber, filter.PageSize);
        }

        // Get your own profile (Admin - Basic)
        // -- ID from _currentTenantUserService
        public async Task<Response<UserDto>> GetProfileDetailsAsync()
        {

            var currentUser = await _userManager.Users.Where(x => x.Id == _currentTenantUserService.UserId).FirstOrDefaultAsync();

            var roles = _userManager.GetRolesAsync(currentUser);
            var roleId = roles.Result.FirstOrDefault();
            currentUser.RoleId = roleId;

            var dto = new UserDto();
            var dtoUser = _mapper.Map(currentUser, dto);

            return Response<UserDto>.Success(dtoUser);
        }

        // Get a user to edit (Admin)
        // -- ID from guid in request
        public async Task<Response<UserDto>> GetUserDetailsAsync(Guid id)
        {
            var user = await _userManager.Users.Where(x => x.Id == Convert.ToString(id)).FirstOrDefaultAsync();

            if (user == null)
                return Response<UserDto>.Fail("User doesn't exist");

            var roles = _userManager.GetRolesAsync(user);
            var roleId = roles.Result.FirstOrDefault();
            user.RoleId = roleId;

            var responseDto = _mapper.Map(user, new UserDto());

            return Response<UserDto>.Success(responseDto);

        }

        // Create a new user (Admin)
        public async Task<Response<UserDto>> RegisterAsync(RegisterUserRequest request)
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);
            if (userExist != null)
                return Response<UserDto>.Fail("User already exists");

            var user = new ApplicationUser
            {
                UserName = request.Email + "." + _currentTenantUserService.TenantId + "." + NanoHelpers.GenerateHex(4), // must be unique across all tenants
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                EmailConfirmed = true,
                IsActive = true,
                DarkModeDefault = true,
                PageSizeDefault = 10,
                TenantId = _currentTenantUserService.TenantId // manually assign tenantId -- applicationUser class doesnt implement IMustHaveTenant interface

            };

            var result = await _userManager.CreateAsync(user, request.Password); // create user with password
            if (result.Succeeded)
            {

                await _userManager.AddToRoleAsync(user, request.RoleId);
                user.RoleId = request.RoleId; // populate the role id value for the response object

                var responseDto = _mapper.Map(user, new UserDto()); // map the application user (with role) to a user dto for response

                return Response<UserDto>.Success(responseDto);

            }
            else
            {
                var messages = new List<string>();
                foreach (var error in result.Errors)
                {
                    messages.Add(error.Description);
                }
                return Response<UserDto>.Fail(messages);

            }

        }

        // Update your profile
        // -- UpdateProfileRequest excludes IsActive / RoleId fields
        public async Task<Response<UserDto>> UpdateProfileAsync(UpdateProfileRequest request)
        {
            // check current user ID
            //            var userInDb = await _userManager.FindByIdAsync(_currentTenantUserService.UserId);

            var userInDb = await _userManager.FindByIdAsync(_currentTenantUserService.UserId);
            if (userInDb == null)
                return Response<UserDto>.Fail("User Not Found");

            // check email in the request
            var userWithEmail = await _userManager.FindByEmailAsync(request.Email);

            // set to null if match is the current user
            if (userWithEmail == userInDb)
            {
                userWithEmail = null;
            }

            // check if email is in use already
            if (userWithEmail != null)
            {
                return Response<UserDto>.Fail("Email already in use");
            }

            string currentImage = userInDb.ImageUrl ?? "";

            // Update all fields
            var updatedAppUser = _mapper.Map(request, userInDb);

            // Image
            if (request.ImageFile != null)
            {
                var imageResult = await _imageService.AddImage(request.ImageFile, 300, 300);
                updatedAppUser.ImageUrl = imageResult;

                if (currentImage != "")
                {
                    await _imageService.DeleteImage(currentImage);
                }

            }

            if (request.DeleteCurrentImage == true && currentImage != "")
            {
                await _imageService.DeleteImage(currentImage);
                updatedAppUser.ImageUrl = "";
            }

            // Save
            await _userManager.UpdateAsync(updatedAppUser);

            // Response DTO
            var responseDto = _mapper.Map(updatedAppUser, new UserDto());
            responseDto.RoleId = _userManager.GetRolesAsync(updatedAppUser).Result.FirstOrDefault();

            return Response<UserDto>.Success(responseDto);
        }

        // Admin update user
        public async Task<Response<UserDto>> UpdateUserAsync(UpdateUserRequest request, Guid id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return Response<UserDto>.Fail("Not found");


            var isCurrentAdminUser = false; // since its possible to put in your own GUID, must check if this is current admin user
            if (id == Guid.Parse(_currentTenantUserService.UserId))
            {
                isCurrentAdminUser = true;
                request.IsActive = true; // prevent setting current user to inactive
            }


            var updatedUser = _mapper.Map(request, user); // map to application user           
            var result = await _userManager.UpdateAsync(updatedUser);

            if (result.Succeeded)
            {
                var updatedUserDTO = _mapper.Map(updatedUser, new UserDto()); // map to userDto for response object

                if (!isCurrentAdminUser) // restrict editing your own role
                {

                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

                    await _userManager.AddToRoleAsync(user, request.RoleId); // remove from all roles, then add to new role
                }

                return Response<UserDto>.Success(updatedUserDTO);

            }
            else
            {
                var messages = new List<string>();
                foreach (var error in result.Errors)
                {
                    messages.Add(error.Description);
                }
                return Response<UserDto>.Fail(messages);

            }
        }

        // Change Preferences of currently logged in user
        public async Task<Response<string>> ChangePreferencesAsync(UpdatePreferencesRequest request)
        {
            var user = await _userManager.FindByIdAsync(_currentTenantUserService.UserId);

            if (user == null)
            {
                return Response<string>.Fail("Not found");
            }

            var updatedUser = _mapper.Map(request, user);
            var result = await _userManager.UpdateAsync(updatedUser);

            if (result.Succeeded)
            {
                return Response<string>.Success("Preferences updated");
            }
            else
            {
                return Response<string>.Fail("Error in changing user preferences");
            }
        }

        // Change Password of currently logged in user
        public async Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(_currentTenantUserService.UserId);
            if (user == null)
            {
                return Response<string>.Fail("Not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
            if (result.Succeeded)
            {
                return Response<string>.Success("Password updated");
            }
            else
            {
                return Response<string>.Fail("Error in changing password");
            }
        }

        // Forgot Password (anonymous)
        public async Task<Response<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {

            var user = await _userManager.FindByEmailAsync(request.Email.Normalize());
            if (user is null || user.IsActive == false)
            {
                return Response<string>.Fail("Not found");
            }


            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var endpointUri = "https://localhost:3000/reset-password"; // react client link

            //const string route = "identity/reset-password";
            //var endpointUri = new Uri(string.Concat($"{origin}/", route));
            //string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);

            var mailRequest = new MailRequest
            {
                To = request.Email,
                Subject = "Reset Password",
                Body = $"Your Password Reset Token is '{code}'. You can reset your password using the {endpointUri} Endpoint.",
            };


            await _mailService.SendAsync(mailRequest);

            return Response<string>.Success("Password Reset Mail has been sent to your authorized Email.");
        }

        // Reset Password (anonymous)
        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || user.IsActive == false)
            {
                return Response<string>.Fail("Not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return Response<string>.Success("Password reset");
            }
            else
            {
                return Response<string>.Fail("Password reset fail");
            }
        }

        // Delete User
        public async Task<Response<Guid>> DeleteUserAsync(Guid id)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null)
                    return Response<Guid>.Fail("Not found");

                await _userManager.DeleteAsync(user);

                return Response<Guid>.Success(id);
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }


        }
    }
}
