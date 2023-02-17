// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoSingular.Infrastructure.Identity;

namespace NanoSingular.RazorApi.Pages.Authentication
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager; //this was
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public string Tenant { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (returnUrl == "/")
            {
                returnUrl = "~/Venues/Index";
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = null;
                var userName = Input.Email;
                if (IsValidEmail(Input.Email)) // allow login with email too
                {
                    user = await _userManager.FindByEmailAsync(Input.Email);

                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }

                    userName = user.UserName;
                }

                //https://blog.dangl.me/archive/adding-custom-claims-when-logging-in-with-aspnet-core-identity-cookie/
                var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, Input.Password);

                if (!passwordIsCorrect)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                // adding custom tenant claim, this can potentially be moved to CustomClaimsCookieSignInHelper
                // as mentioned in the blog
                var customClaims = new[]
                {
                    new Claim("logged_in_day", DateTime.UtcNow.DayOfWeek.ToString()),
                    new Claim("tenant", Input.Tenant)
                };

                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
                {
                    claimsIdentity.AddClaims(customClaims);
                }

                // see SignInAsync call with claims after the result.Succeeded condition
                // END adding additional claims

                // we don't really need this line, but we need to check the result, not sure if there is any alternative way of doing that
                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // this will set the context with additional claims
                    await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return Page();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}