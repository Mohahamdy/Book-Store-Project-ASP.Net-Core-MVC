using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Filters;
using Project.Models;
using Project.ViewModels;

using System.Security.Claims;

using System.Text.Encodings.Web;
using System.Threading.Channels;

namespace Project.Controllers
{

    public class AccountController : Controller
    {
        #region Inject Services
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ISenderEmail emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISenderEmail emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }
        #endregion

        #region Rgisteration
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RedirectAuthenticatedUsersAttribute]

        public async Task<IActionResult> Register(UserRegisterViewModel userRegisterVM)
        {
            if (ModelState.IsValid)
            {
                //Mapping viewModel
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = userRegisterVM.Email,
                    FirstName = userRegisterVM.FirstName,
                    LastName = userRegisterVM.LastName,
                    Email = userRegisterVM.Email,
                    PasswordHash = userRegisterVM.Password,
                    Address = userRegisterVM.Address,
                    PhoneNumber = userRegisterVM.Phone,
                    image = "Defualt.png"
                };

                //create user in DB using userManger(service controls the operations on user)
                IdentityResult result = await userManager.CreateAsync(user, userRegisterVM.Password);

                if (result.Succeeded)
                {
                    //send confirmation email
                    await SendConfirmationEmail(userRegisterVM.Email, user);

                    /*//make cookie foe the user
                    await signInManager.SignInAsync(user, false);*/
                    return View("RegistrationSuccessful");
                }

                //add errors to the modelState
                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item.Description);
            }

            return View("Register", userRegisterVM);
        }
        #endregion

        #region ConfirmationEmail
        //Action of Confirmation result
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string? Email, string Token)
        {
            if (Email == null || Token == null)
            {
                ViewBag.Message = "The link is Invalid or Expired";
                return View();
            }


            //Find user by id
            ApplicationUser user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User Email {Email} is Invalid";
                return NotFound();
            }

            //Call the ConfirmEmailAsync Method which will mark the Email as Confirmed
            var result = await userManager.ConfirmEmailAsync(user, Token);
            if (result.Succeeded)
            {
                ViewBag.Message = "Thank you for confirming your email";
                return View();
            }

            ViewBag.Message = "Email cannot be confirmed";
            return View();
        }
        #endregion

        #region Resend Confirmation Email
        //action opens the view(form) of resending confiramtion Email
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResendConfirmationEmail(bool IsResend = true)
        {
            if (IsResend)
            {
                ViewBag.Message = "Resend Confirmation Email";
            }
            else
            {
                ViewBag.Message = "Send Confirmation Email";
            }
            return View();
        }

        //resend confirmation Email 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmationEmail(string Email)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(Email);

            if (user == null || await userManager.IsEmailConfirmedAsync(user))
            {
                return View("ConfirmationEmailSent");
            }

            await SendConfirmationEmail(Email, user);
            return View("ConfirmationEmailSent");
        }
        #endregion

        #region Log in
        [HttpGet]
        public async Task<IActionResult> LogIn(int? bookID, string? ReturnUrl = null)
        {
            ViewBag.Confirm = true;
            ViewBag.bookID = bookID;

            UserLoginViewModel model = new UserLoginViewModel()
            {
                ReturnUrl = ReturnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View("Login", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RedirectAuthenticatedUsersAttribute]

        public async Task<IActionResult> Login(UserLoginViewModel userLoginVM, int? bookID)
        {
            ViewBag.Confirm = true;
            userLoginVM.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                ApplicationUser userFromDb = await userManager.FindByEmailAsync(userLoginVM.Email);

                if (userFromDb != null)
                {
                    if (userFromDb.EmailConfirmed == false)
                    {
                        ModelState.AddModelError("Email", "Email not confirmed yet, Press Confirm Email.");
                        ViewBag.Confirm = false;
                        return View("Login", userLoginVM);
                    }
                    bool founded = await userManager.CheckPasswordAsync(userFromDb, userLoginVM.Password);
                    if (founded)
                    {
                        //add cookie extra info

                        List<Claim> claims = [new Claim("image", userFromDb.image)];



                        await signInManager.SignInWithClaimsAsync(userFromDb, userLoginVM.RememberMe, claims);
                        if (bookID == null)
                        {
                            return RedirectToAction("index", "Home");
                        }
                        else
                            return RedirectToActionPermanent("BookDetails", "Home", new { id = bookID });
                    }
                    else
                        ModelState.AddModelError("Password", "Incorrect Password, Check your Password.");
                }
                else
                    ModelState.AddModelError("Email", "Incorrect Email, Check Your Email.");
            }
            return View("Login", userLoginVM);
        }
        #endregion

        #region Extrnal Login
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            //This call will generate a URL that directs to the ExternalLoginCallback action method in the Account controller
            //with a route parameter of ReturnUrl set to the value of returnUrl.
            var redirectUrl = Url.Action(action: "ExternalLoginCallback", controller: "Account", values: new { ReturnUrl = returnUrl });
            // Configure the redirect URL, provider and other properties
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //This will redirect the user to the external provider's login page
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            UserLoginViewModel loginViewModel = new UserLoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            // If the user already has a login (i.e., if there is a record in AspNetUserLogins table)
            // then sign-in the user with this external login provider
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // If there is no record in AspNetUserLogins table, the user may not have a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                            image = "Defualt.png",
                            Address = "Cairo"
                        };

                        //This will create a new user into the AspNetUsers table without password
                        await userManager.CreateAsync(user);
                    }

                    // Add a login (i.e., insert a row for the user in AspNetUserLogins table)
                    await userManager.AddLoginAsync(user, info);

                    //Then Signin the User
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on info@dotnettutorials.net";

                return View("Error");
            }
        }
        #endregion

        #region Log Out
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            //Destroy cookie
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion

        #region Forgot Password Confirmation
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && user.EmailConfirmed == true)
                {
                    await SendForgotPasswordEmail(user.Email, user);
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region Reset Password Confirmation
        // This action method will be invoked when the user clicks on the Password Reset Link in his/her email. and takes email and token from the link
        [HttpGet]
        public IActionResult ResetPassword(string? Email, string? Token)
        {
            if (Email == null && Token == null)
            {
                ViewBag.ErrorTitle = "Invalid Password Reset Token";
                ViewBag.ErrorMessage = "The Link is Expired or Invalid";
                return View("Error");
            }
            else
            {
                ResetPasswordViewModel viewModel = new ResetPasswordViewModel();
                viewModel.Email = Email;
                viewModel.Token = Token;
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }

                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View(model);
                }

                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region Two Private Method to send EmailConfirmation And ResetPasswordConfirmation

        //Private Method Which will send confirmation email to user
        private async Task SendConfirmationEmail(string? email, ApplicationUser user)
        {
            //Generate token 
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            //Build the Email Confirmation Link
            var ConfirmationLink = Url.Action("ConfirmEmail", "Account",
                new { Email = user.Email, Token = token }, protocol: HttpContext.Request.Scheme);

            //Send the Confirmation Email to the User Email
            await emailSender.SendEmailAsync(email, "Confirm Your Email", $"Please Confirm Your Account by <a href='{HtmlEncoder.Default.Encode(ConfirmationLink)}'>Clicking here</a>.", true);
        }

        //private method which will send reset password email
        private async Task SendForgotPasswordEmail(string? email, ApplicationUser? user)
        {
            //Generate Reset Password Token
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            //Generate Reset Password Link
            var ResetPasswordLink = Url.Action("ResetPassword", "Account"
                , new { Email = email, Token = token }, protocol: HttpContext.Request.Scheme);

            //send reset password Email to user
            await emailSender.SendEmailAsync(email, "Reset Your Password"
                , $"Please Reset Your Passowrd by <a href='{HtmlEncoder.Default.Encode(ResetPasswordLink)}'>Clicking here</a>", true);
        }
        #endregion
    }
}
