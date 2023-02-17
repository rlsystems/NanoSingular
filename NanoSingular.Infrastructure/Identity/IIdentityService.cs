using NanoSingular.Application.Common.Marker;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Infrastructure.Identity.DTOs;

namespace NanoSingular.Infrastructure.Identity
{
    public interface IIdentityService : IScopedService
    {
        Task<Response<IEnumerable<UserDto>>> GetUsersAsync(); // non-paged full list

        Task<PaginatedResponse<UserDto>> GetUsersPaginatedAsync(UserListFilter filter); // paged / filtered list

        Task<Response<UserDto>> GetProfileDetailsAsync(); // get your own

        Task<Response<UserDto>> GetUserDetailsAsync(Guid id); // admin get user

        Task<Response<UserDto>> RegisterAsync(RegisterUserRequest request); // admin create a new user

        Task<Response<UserDto>> UpdateProfileAsync(UpdateProfileRequest request); // update your own

        Task<Response<UserDto>> UpdateUserAsync(UpdateUserRequest request, Guid id); // admin update a user

        Task<Response<string>> ChangePreferencesAsync(UpdatePreferencesRequest request); // change your own password

        Task<Response<string>> ChangePasswordAsync(ChangePasswordRequest request); // change your own password

        Task<Response<string>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin); // change your own password

        Task<Response<string>> ResetPasswordAsync(ResetPasswordRequest request); // reset password

        Task<Response<Guid>> DeleteUserAsync(Guid id); // admin delete a user
    }
}