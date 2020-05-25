/*
 * Auteurs : Sébastien Berger, Tim Allemann
 * Date : 05.05.2020
 * Détail : Contrôleur séparé pour l'inscription, la confirmation Email ainsi que la vérification SMS
 */
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
using Serilog;
using IdentityServerAspNetIdentity.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Configuration;
using IdentityServerAspNetIdentity.ViewModels;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PhoneNumbers;

namespace IdentityServerAspNetIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly LoveMirroringContext _context;
        private HttpClient client = new HttpClient();
        private readonly IEmailSender _emailSender;
        private readonly TwilioVerifySettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;
        private IConfiguration Configuration { get; }
        private readonly ILogger<AccountController> _logger;
        private readonly IActionContextAccessor _accessor;
        private static PhoneNumberUtil _phoneUtil;

        public AccountController(LoveMirroringContext context, 
                                 IEmailSender emailSender,
                                 IOptions<TwilioVerifySettings> settings,
                                 UserManager<ApplicationUser> userManager, 
                                 IConfiguration configuration,
                                 ILogger<AccountController> logger,
                                 IActionContextAccessor accessor)
        {
            _context = context;
            _emailSender = emailSender;
            _settings = settings.Value;
            _userManager = userManager;
            Configuration = configuration;
            _logger = logger;
            _accessor = accessor;
            _phoneUtil = PhoneNumberUtil.GetInstance();
        }

        // Permet d'afficher l'inscription avec plusieurs données
        public async Task<IActionResult> SignUp()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string sexes = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/Sex");
            string corpulences = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/corpulences");
            string hairSize = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/hairSize");
            string hairColor = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/hairColor");
            string sexuality = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/sexuality");
            string styles = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/styles");
            string religions = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/religions");

            List<Religion> resultReligion = JsonConvert.DeserializeObject<List<Religion>>(religions);
            List<Sex> resultSexes = JsonConvert.DeserializeObject<List<Sex>>(sexes);
            List<Corpulence> resultCorpulences = JsonConvert.DeserializeObject<List<Corpulence>>(corpulences);
            List<HairColor> resultHairColors = JsonConvert.DeserializeObject<List<HairColor>>(hairColor);
            List<HairSize> resultHairSizes = JsonConvert.DeserializeObject<List<HairSize>>(hairSize);
            List<Sexuality> resultSexualities = JsonConvert.DeserializeObject<List<Sexuality>>(sexuality);
            List<Style> resultStyle = JsonConvert.DeserializeObject<List<Style>>(styles);

            ViewData["sexes"] = resultSexes;
            ViewData["corpulences"] = resultCorpulences;
            ViewData["hairColors"] = resultHairColors;
            ViewData["hairSizes"] = resultHairSizes;
            ViewData["sexualities"] = resultSexualities;
            ViewData["styles"] = resultStyle;
            ViewData["religions"] = resultReligion;

            string ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation("A User is trying to sign up with ip : " + ip);
            UserTrace trace = new UserTrace
            {
                Logdate = DateTime.Now,
                Ipadress = ip,
                Pagevisited = "SignUp : A User is trying to sign up"
            };
            _context.UserTraces.Add(trace);
            _context.SaveChanges();

            return View();
        }

        // Traitement de l'inscription
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

                        var checkUser = _userManager.FindByNameAsync(input.UserName).Result;
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

                            var checkEmail = _userManager.FindByEmailAsync(input.Email).Result;

                            if(checkEmail != null)
                            {
                                throw new Exception("Email déjà utilisé");
                            }

                            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                            int dob = int.Parse(input.Birthday.ToString("yyyyMMdd"));
                            int age = (now - dob) / 10000;


                            if (age < 18)
                            {
                                throw new Exception("Vous devez avoir 18 ans pour vous inscrire");
                            }

                            // Vérification du numéro de téléphone - Sébastien Berger
                            PhoneNumber phoneNumber = _phoneUtil.Parse(input.PhoneNumber, input.countryCode);

                            if(!_phoneUtil.IsValidNumberForRegion(phoneNumber, input.countryCode) && !phoneNumber.HasExtension)
                            {
                                throw new Exception("Numéro invalide");
                            }
                            else
                            {
                                input.PhoneNumber = "+" + phoneNumber.CountryCode.ToString() + phoneNumber.NationalNumber.ToString();
                            }
                            
                            user.CorpulenceId = input.CorpulenceId;
                            user.SexualityId = input.SexualityId;
                            user.Sexeid = input.Sexeid;
                            user.HairColorId = input.HairColorId;
                            user.HairSizeId = input.HairSizeId;
                            user.QuizCompleted = false;
                            user.ReligionId = input.ReligionId;
                            user.AccountCompleted = true;
                            checkUser = user;

                            var result = userMgr.CreateAsync(checkUser, user.PasswordHash).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            else
                            {
                                AspNetUserRole userRole = new AspNetUserRole();
                                userRole.UserId = checkUser.Id;
                                userRole.RoleId = "Utilisateur";

                                _context.AspNetUserRoles.Add(userRole);
                                string ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
                                _logger.LogInformation("User created a new account with password with ip: " + ip);

                                string userId = _context.AspNetUsers.Where(u => u.UserName == input.UserName).Select(u => u.Id).SingleOrDefault();
                                UserTrace trace = new UserTrace
                                {
                                    Logdate = DateTime.Now,
                                    Ipadress = ip,
                                    Pagevisited = "SignUp : User created a new account",
                                    Id = userId
                                };
                                UserStyle us = new UserStyle();
                                us.Id = user.Id;
                                us.StyleId = input.StyleId;

                                _context.UserStyles.Add(us);
                                _context.UserTraces.Add(trace);
                                _context.SaveChanges();

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
                Log.Debug($"{e.Message} error");
                ViewData["error"] = e.Message;

                await SignUp();
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

        // Traiter l'oubli du mot de passe
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

        // Retourne la vue quand on a perdu son mot de passe
        [Route("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Traitement de l'envoi d'un nouveau mot de passe
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

        // Reset du password
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

        public async Task<IActionResult> VerifyPhoneAsync()
        {
            await LoadPhoneNumber();
            SMSVerification model = new SMSVerification();
            model.PhoneNumber = PhoneNumber;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostVerifyPhoneModelAsync() 
        {
            await LoadPhoneNumber();
            SMSVerification model = new SMSVerification();
            try
            {
                VerificationResource verification = await VerificationResource.CreateAsync(
                    to: PhoneNumber,
                    channel: "sms",
                    pathServiceSid: _settings.VerificationServiceSID
                );

                if (verification.Status == "pending") 
                {
                    model.PhoneNumber = PhoneNumber;
                    return View("ConfirmPhone", model);
                }

                ModelState.AddModelError("", $"Your verification is not pending, please constact admin");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "There was an error sending the verification code, please contact admin");
            }
            return View("VerifyPhone", model);
        }

        [HttpPost]
        public async Task<IActionResult> PostVerifyPhoneCodeAsync(SMSVerification input) {
            await LoadPhoneNumber();
            if (!ModelState.IsValid) 
            { 
                return View("ConfirmPhone", input); 
            }

            try
            {
                VerificationCheckResource verification = await VerificationCheckResource.CreateAsync(
                    to: PhoneNumber,
                    code: input.VerificationCode,
                    pathServiceSid: _settings.VerificationServiceSID
                );
                if (verification.Status == "approved")
                {
                    var identityUser = await _userManager.GetUserAsync(User);
                    identityUser.PhoneNumberConfirmed = true;
                    var updateResult = await _userManager.UpdateAsync(identityUser);

                    if (updateResult.Succeeded)
                    {
                        return View("ConfirmPhoneSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"There was an error confirming the verification code: {verification.Status}");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error confirming the code, please check the verification code is correct and try again");
            }

            return View("ConfirmPhone", input);
        }

        public string PhoneNumber { get; set; }
        private async Task LoadPhoneNumber ()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID");
            }
            PhoneNumber = user.PhoneNumber;
        }
    }
}