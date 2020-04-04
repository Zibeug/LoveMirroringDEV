using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityServerAspNetIdentity.Models;
using Microsoft.Extensions.DependencyInjection;
using IdentityServerAspNetIdentity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;
using Serilog;
using IdentityServerAspNetIdentity.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Configuration;
using IdentityServerAspNetIdentity.ViewModels;
using System.Text.Encodings.Web;

namespace IdentityServerAspNetIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [Route("SignUpSend", Name = "SignUpSend")]
        [HttpPost]
        public async Task<IActionResult> SignUpSend(RegisterInput input)
        {
            ApplicationUser user = new ApplicationUser();
            try
            {
                string connectionString = Startup.Configuration.GetConnectionString("DefaultConnection");
                var services = new ServiceCollection();
                services.AddLogging();
                services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(connectionString));

                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                        var checkUser = userMgr.FindByNameAsync(input.UserName).Result;
                        if (checkUser == null)
                        {
                            if(input.ConfirmPassword != input.PasswordHash)
                            {
                                throw new Exception("Votre formulaire comporte des erreurs");
                            }
                            else
                            {
                                user = input;
                            }
                            user.Sexeid = 1;
                            user.QuizCompleted = false;
                            checkUser = user;
                            var result = userMgr.CreateAsync(checkUser, user.PasswordHash).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            else
                            {
                                //_logger.LogInformation("User created a new account with password.");

                                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, Request.Scheme);

                                string message = "Salut mon pote comment ca va ? si tu veux confirmer ton inscription c'est par <a href='" + callbackUrl + "'>ici</a>";
                                await _emailSender.SendEmailAsync(user.Email, "Confirmer votre Email", message);

                            }
                          
                            Log.Debug($"{checkUser.UserName} created");
                        }
                        else
                        {
                            throw new Exception("Votre nom d'utilisateur est déjà pris");
                        }
                    }
                }

                return View("SignUpSuccess", user);
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
                return View("SignUp");
            }
            
            
        }

        public IActionResult Cancel()
        {
            return View("Login");
        }

        /*Validation par Email*/
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            // StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming you email";
            return View("ConfirmEmail");
        }

        [Route("ForgotPassword", Name = "ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordInput input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, Request.Scheme);

                await _emailSender.SendEmailAsync(
                    input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return View("ForgotPasswordConfirmation");
            }

            return View();
        }

        [Route("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotSend", Name = "ForgotSend")]
        [HttpPost]
        public async Task<IActionResult> ForgotSend(ForgotPasswordInput Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return View("ForgotPasswordConfirmation");
            }

            return View("ResetPasswordFail");
        }

        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return View("ResetPasswordFail");
            }
            else
            {
                ResetPasswordInput test = new ResetPasswordInput
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return View("ResetPassword");
            }
        }

        [Route("ChangePassword", Name = "ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordInput input)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPasswordFail");
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("ResetPasswordFail");
            }
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, input.Password);
            if (!result.Succeeded)
            {
                return View("ResetPasswordFail");
            }

            return View("ResetPasswordConfirmation");
        }
    }
}