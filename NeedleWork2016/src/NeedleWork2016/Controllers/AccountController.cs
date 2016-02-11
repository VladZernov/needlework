using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using NeedleWork2016.Models;
using System.Net.Mail;
using System.Security.Claims;
using System;
using NeedleWork2016.Core;
using NeedleWork2016.Repository;

namespace NeedleWork2016.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly Core.Error.ListOfErrors _listOfErrors;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _listOfErrors = Core.Error.ErrorStorage.GetListOfErrors();
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }

        [HttpPost]
        [AllowAnonymous]
        //Method for user LogIn
        public async Task<string> Login(string email, string password, bool remember)
        {
            try
            {
                //Сhecking e-mail on the registered
                if (UserProfileRepository.UserIsRegistered(email))
                {
                    //Checking user email confirm
                    if (!UserProfileRepository.IsUserConfirmedEmail(email))
                        return _listOfErrors[1002].ToJson();

                    if (email == null || password == null)
                        return _listOfErrors[1003].ToJson();

                    //LogIn user
                    var result = await _signInManager.PasswordSignInAsync(email, password, remember, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return _listOfErrors[2001].ToJson();
                    }
                    else
                        return _listOfErrors[1004].ToJson();

                }
                return _listOfErrors[1005].ToJson();
            }
            catch (Exception ex)
            {
                return new Core.Error.Error(ex.HResult, ex.Source, ex.Message).ToJson();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        //Method for user registration
        public async Task<string> Register(string firstName, string lastName, string email, string password, string captchaResponse)
        {
            try
            {
                //CAPTCHA validation
                bool IsCaptchaValid = reCaptchaClass.Validate(captchaResponse) == "True" ? true : false;

                if (IsCaptchaValid)
                {                    
                    //Checking user data
                    if (firstName == null || lastName == null)
                        return _listOfErrors[1007].ToJson();

                    if (email == null)
                        return _listOfErrors[1008].ToJson();

                    //Сhecking e-mail on the registered
                    if (UserProfileRepository.UserIsRegistered(email))
                    {
                        return _listOfErrors[1009].ToJson();
                    }

                    //Creating new user
                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        EmailConfirmed = false
                    };

                    var result = await _userManager.CreateAsync(user, password);

                    //Sending confirmation email of registration
                    if (result.Succeeded)
                    {
                        //Add user role
                        await _userManager.AddToRoleAsync(user, "User");

                        //Generation message body
                        var callbackUrl = GetLinkForUser(user, "emailconfirmed", "Account", "ConfirmEmail");

                        //Add message tempalate
                        string template = ResourceReader.GetTemplate("..//Templates//Message.cshtml").Replace("@User", firstName);

                        //Sending message to user e-mail
                        SendEmail(email, "Confirm your account", template + "<a href=\"" + callbackUrl + "\">Registration confirmation</a>");

                        //User LogOff
                        await _signInManager.SignOutAsync();

                        return _listOfErrors[2002].ToJson();
                    }
                    else
                    {
                        string errors = "";

                        //Get list of errors
                        foreach (var i in result.Errors)
                            errors += new Core.Error.Error(int.Parse(i.Code), "Error", i.Description).ToJson();

                        return errors;
                    }
                }
                else
                    return _listOfErrors[1001].ToJson();
            }
            catch (Exception ex)
            {
                return new Core.Error.Error(ex.HResult, ex.Source, ex.Message).ToJson();
            }
        }

        [HttpPost]
        //Method for user logOff
        public void LogOff()
        {
            _signInManager.SignOutAsync();
        }

        [HttpGet]
        [AllowAnonymous]
        //Method for confirm user mail 
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            //Checking userId and email identity code
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { parametr = _listOfErrors[1006].ToJson() });
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                UserProfileRepository.EmailConfirm(user.Email);
            }
            else
                return RedirectToAction(nameof(HomeController.Index), "Home", new { parametr = _listOfErrors[1010].ToJson() });

            return RedirectToAction(nameof(HomeController.Index), "Home", new { parametr = _listOfErrors[2003].ToJson() });
        }

        //Method that send message with body, subject
        private void SendEmail(string emailid, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Timeout = 0;

            System.Net.NetworkCredential credentials =
              new System.Net.NetworkCredential("needlework2016@gmail.com", "24912696");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("needlework2016@gmail.com");
            msg.To.Add(new MailAddress(emailid));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }

        [HttpPost]
        [AllowAnonymous]
        //Method to send message after entering the mail for the page forgot password
        public async Task<string> ForgotPassword(string email)
        {
            try
            {
                //Checking mail is register
                if (UserProfileRepository.UserIsRegistered(email))
                {
                    var user = await _userManager.FindByNameAsync(email);

                    //Generation message body
                    var callbackUrl = GetLinkForUser(user, "resetPassword", "Home", "Index");

                    //Add message template
                    string template = ResourceReader.GetTemplate("..//Templates//ResetPasswordMessage.cshtml").Replace("@User", user.FirstName);

                    //Sending message
                    SendEmail(email, "Change your password", template + "<a href=\"" + callbackUrl + "\">Reset password</a>");

                    return _listOfErrors[2006].ToJson();
                }
                return
                    _listOfErrors[1005].ToJson();
            }
            catch (Exception ex)
            {
                return new Core.Error.Error(ex.HResult, ex.Source, ex.Message).ToJson();
            }
        }

        //get link for user 
        public string GetLinkForUser(ApplicationUser user, string parametr, string controller, string action)
        {
            var code =  _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action(action, controller, null, protocol: HttpContext.Request.Scheme);
            return url + string.Format("/?userId={0}&code={1}&parametr={2}", user.Id, code.Result, parametr);
        }

        [HttpPost]
        [AllowAnonymous]
        //Method for reset password
        public async Task<string> ResetPassword(string email, string password, string code)
        {
            try
            {
                //Checking e-mail identity code
                if (code == null)
                    return _listOfErrors[1006].ToJson();

                if (password == null)
                    return _listOfErrors[1004].ToJson();

                //Check e-mail on the registered
                if (UserProfileRepository.UserIsRegistered(email))
                {
                    var user = await _userManager.FindByNameAsync(email);

                    //Reset user password
                    var result = await _userManager.ResetPasswordAsync(user, code, password);

                    if (result.Succeeded)
                    {
                        //User logIn
                        await _signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);
                        return _listOfErrors[2007].ToJson();
                    }
                    else
                    {
                        string errors = "";

                        //Get list of errors
                        foreach (var i in result.Errors)
                            errors += new Core.Error.Error(int.Parse(i.Code), "Error", i.Description).ToJson();

                        return errors;
                    }
                }

                return
                    _listOfErrors[1005].ToJson();
            }
            catch (Exception ex)
            {
                return new Core.Error.Error(ex.HResult, ex.Source, ex.Message).ToJson();
            }
        }

        [HttpPost]
        //Method for change password
        public async Task<string> ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                //Get current user
                var user = await _userManager.FindByNameAsync(User.GetUserName());

                //Changing user password
                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                if (result.Succeeded)
                {
                    return _listOfErrors[2007].ToJson();
                }

                return _listOfErrors[1004].ToJson();
            }
            catch (Exception ex)
            {
                return new Core.Error.Error(ex.HResult, ex.Source, ex.Message).ToJson();
            }
        }

        //Dont using
        ////
        //// GET: /Account/ResetPasswordConfirmation
        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}

        // //Not use
        //  //GET: /Account/SendCode
        // [HttpGet]
        // [AllowAnonymous]
        // public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        // {
        //     var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //     if (user == null)
        //     {
        //         return View("Error");
        //     }
        //     var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
        //     var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //     return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        // }

        // // POST: /Account/SendCode
        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> SendCode(SendCodeViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View();
        //     }

        //     var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //     if (user == null)
        //     {
        //         return View("Error");
        //     }

        //     // Generate the token and send it
        //     var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
        //     if (string.IsNullOrWhiteSpace(code))
        //     {
        //         return View("Error");
        //     }

        //     var message = "Your security code is: " + code;
        //     if (model.SelectedProvider == "Email")
        //     {
        //         await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
        //     }
        //     else if (model.SelectedProvider == "Phone")
        //     {
        //         await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
        //     }

        //     return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        // }


        // GET: /Account/VerifyCode
        //[HttpGet]
        //[AllowAnonymous]
        // public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        // {
        //     // Require that the user has already logged in via username/password or external login
        //     var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //     if (user == null)
        //     {
        //         return View("Error");
        //     }
        //     return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        // }

        // //
        // // POST: /Account/VerifyCode
        // [HttpPost]
        // [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }

        //     // The following code protects for brute force attacks against the two factor codes.
        //     // If a user enters incorrect codes for a specified amount of time then the user account
        //     // will be locked out for a specified amount of time.
        //     var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
        //     if (result.Succeeded)
        //     {
        //         return RedirectToLocal(model.ReturnUrl);
        //     }
        //     if (result.IsLockedOut)
        //     {
        //         _logger.LogWarning(7, "User account locked out.");
        //         return View("Lockout");
        //     }
        //     else
        //     {
        //         ModelState.AddModelError("", "Invalid code.");
        //         return View(model);
        //     }
        // }

        //#region Helpers

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //}

        //private async Task<ApplicationUser> GetCurrentUserAsync()
        //{
        //    return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        //}

        //private IActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }
        //}
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public IActionResult ExternalLogin(string provider, string returnUrl = null)
        //{
        //    // Request a redirect to the external login provider.
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return new ChallengeResult(provider, properties);
        //}

        //// GET: /Account/ExternalLoginCallback
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction(nameof(Login));
        //    }

        //    // Sign in the user with this external login provider if the user already has a login.
        //    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    if (result.RequiresTwoFactor)
        //    {
        //        return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        return View("Lockout");
        //    }
        //    else
        //    {
        //        // If the user does not have an account, then ask the user to create an account.
        //        ViewData["ReturnUrl"] = returnUrl;
        //        ViewData["LoginProvider"] = info.LoginProvider;
        //        var email = info.ExternalPrincipal.FindFirstValue(ClaimTypes.Email);
        //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
        //    }
        //}

        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        //{
        //    if (User.IsSignedIn())
        //    {
        //        return RedirectToAction(nameof(ManageController.Index), "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await _signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await _userManager.AddLoginAsync(user, info);
        //            if (result.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //    }

        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View(model);
        //}
        //#endregion
    }
}
