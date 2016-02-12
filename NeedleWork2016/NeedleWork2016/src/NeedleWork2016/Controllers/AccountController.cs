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
using System.Text.RegularExpressions;

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
                if (email == null || password == null)
                    return _listOfErrors[1003].ToJson();
                //Сhecking e-mail on the registered
                if (UserProfileRepository.UserIsRegistered(email))
                {
                    //Checking user email confirm
                    if (!UserProfileRepository.IsUserConfirmedEmail(email))
                        return _listOfErrors[1002].ToJson();

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
                        var callbackUrl = GetLinkForUser(user, _listOfErrors[2003], "Account", "ConfirmEmail");

                        //Add message tempalate
                        string template = ResourceReader.GetTemplate("..//Templates//RegistrationConfirmationMessage.cshtml").Replace("@User", firstName);

                        //Sending message to user e-mail
                        SendEmail(email, "Confirm your account", template + "<a href=\"" + callbackUrl + "\">Registration confirmation</a>");

                        //User LogOff
                        await _signInManager.SignOutAsync();

                        return _listOfErrors[2002].ToJson();
                    }
                    else
                    {
                        return PasswordValidation(password);
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
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (UserProfileRepository.IsUserConfirmedEmail(user.Email))
                return RedirectToAction(nameof(HomeController.Index), "Home", new { successCode = _listOfErrors[2003].Code });

            if (user != null)
            {
                UserProfileRepository.EmailConfirm(user.Email);
            }
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return RedirectToAction(nameof(HomeController.Index), "Home", new { successCode = _listOfErrors[2003].Code });
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
                    var callbackUrl = GetLinkForUser(user, _listOfErrors[2007], "Home", "Index");

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
                        return PasswordValidation(password);
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

        //#region Helpers
        //get link for user 
        public string GetLinkForUser(ApplicationUser user, Core.Error.Error parametr, string controller, string action)
        {
            var code = _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action(action, controller, null, protocol: HttpContext.Request.Scheme);
            return url + string.Format("/?userId={0}&code={1}&successCode={2}", user.Id, code.Result, parametr.Code);
        }

        //Getting message by code 
        [HttpPost]
        [AllowAnonymous]
        public string GetMessage(int code)
        {
            return _listOfErrors[code].ToJson();
        }

        //Method for e-mail validation
        public string EmailValidation(string email)
        {
            Core.Error.Error error = new Core.Error.Error();

            if (email==null)
                error.Definition += _listOfErrors[1017].ToJson();

            Regex reg = new Regex(@"^([a-z0-9_-]+\.)*[a-z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$");
                if (!reg.Match(email).Success)
                    error.Definition += _listOfErrors[1016].ToJson();
           
            return error.ToJson();
        }

        //Method for password validation
        public string PasswordValidation(string password)
        {
            Core.Error.Error error = new Core.Error.Error();
            error.Name = "Password error";

            if (password == "")
                error.Definition += _listOfErrors[1015].Definition + "<br/>";

            Regex reg;

            if (password.Length < 6)
                error.Definition += _listOfErrors[1011].Definition + "<br/>";

            reg = new Regex(@"[a-z]{1,}");
            if (!reg.Match(password).Success)
                error.Definition += _listOfErrors[1013].Definition + "<br/>";

            reg = new Regex(@"[A-Z]{1,}");
            if (!reg.Match(password).Success)
                error.Definition += _listOfErrors[1014].Definition + "<br/>";

            reg = new Regex(@"[\W]{1,}");
            if (!reg.Match(password).Success)
                error.Definition += _listOfErrors[1012].Definition + "<br/>";

            return error.ToJson();
        }
        //end region
    }
}
