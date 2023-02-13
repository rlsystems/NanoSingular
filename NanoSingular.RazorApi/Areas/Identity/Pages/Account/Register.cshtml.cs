// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Identity.DTOs;
using NanoSingular.Infrastructure.Utility;

namespace NanoSingular.RazorApi.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger
            )
        {
            _userManager = userManager;
   
            _signInManager = signInManager;
            _logger = logger;
         
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {


                //------
                var userExist = await _userManager.FindByEmailAsync(Input.Email);
                if (userExist != null)
                    _logger.LogInformation("User already exists");

                var user = new ApplicationUser
                {
                    UserName = Input.Email + "." + NanoHelpers.GenerateHex(4), // must be unique across all tenants
                    FirstName = "test",
                    LastName = "lastname",
                    PhoneNumber = "992333444",
                    Email = Input.Email,
                    EmailConfirmed = true,
                    IsActive = true,
                    DarkModeDefault = true,
                    PageSizeDefault = 10,
                };

                var result = await _userManager.CreateAsync(user, Input.Password); // create user with password
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, "editor");
                    user.RoleId = "editor"; // populate the role id value for the response object

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }



    }
}
