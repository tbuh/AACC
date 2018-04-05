using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private AACCContext _context;
        public LoginModel(AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; private set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string LoginName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await AuthenticateUser(Input.LoginName, Input.Password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name.ToString(), user.LoginName),
                    new Claim("FullName", user.FullName),
                    new Claim("UserId", user.UserId.ToString()),
                };

                if (user.IsAdmin)
                {
                    claims.Add(new Claim("Admin", ""));
                }
                else if (user.IsSuperAdmin)
                {
                    claims.Add(new Claim("Admin", ""));
                    claims.Add(new Claim("SuperAdmin", ""));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }

            return Page();
        }

        private async Task<ApplicationUser> AuthenticateUser(string login, string password)
        {
            if (login.ToLower() == "admin" && password.ToLower() == "admin")
                return new ApplicationUser
                {
                    IsSuperAdmin = true,
                    FullName = "Super Admin",
                    LoginName = login
                };

            var user = await _context.Assessors.FirstOrDefaultAsync(ass => ass.Login == login && ass.Password == password);

            if (user != null)
            {
                return new ApplicationUser()
                {
                    IsAdmin = user.IsAdmin,
                    FullName = user.Name,
                    LoginName = user.Login,
                    UserId = user.AssessorId
                };
            }
            else
            {
                return null;
            }
        }
    }

    public class ApplicationUser
    {
        public bool IsSuperAdmin { get; set; }
        public string LoginName { get; set; }
        public bool IsAdmin { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
    }
}
