using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DotNetCore.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DotNetCore.Models;

namespace DotNetCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<DotNetCoreUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly DotNetCoreContext _context;

        public LoginModel(SignInManager<DotNetCoreUser> signInManager, ILogger<LoginModel> logger,DotNetCoreContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true                
                var hasher = new PasswordHasher<DotNetCoreUser>();
                DotNetCoreUser user=null;
                bool enabled = false;
                try
                {
                    user = _context.Users.SingleOrDefault(x => x.Email.Equals(Input.Email));
                    enabled = user.Enabled;
                }
                catch(Exception ex)
                {
                    _logger.LogInformation("Something went wrong.Contact the service provider.");
                    return Page();
                }
                var passwordHashing=hasher.VerifyHashedPassword(user, user.PasswordHash, Input.Password);
                Microsoft.AspNetCore.Identity.SignInResult result = null;
                if (passwordHashing == PasswordVerificationResult.Success)
                {
                    if (enabled)
                    {
                        if (user.ToDate > DateTime.Now)
                        {
                            result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);                            
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            _logger.LogInformation("The validity has expired.");
                            return Page();
                            //return RedirectToPage("/Index");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("User in not enabled. Please contact the admin.");
                        return Page();
                        //return RedirectToPage("/Index");
                    }
                }
                if (result.Succeeded)
                {
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
            //return RedirectToPage("/Index");
        }
    }
}
