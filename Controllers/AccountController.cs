using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Accommodation.Models;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using Accommodation.Services.Implementation;
using System.Collections.Generic;
using Microsoft.Owin.Security.DataProtection;
using Org.BouncyCastle.Asn1.Microsoft;

namespace Accommodation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
           
           
                if (!ModelState.IsValid)
            {
                return View(model);
            }
            
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {

                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
    }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RegisterManager()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult RegisterCleaner()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterManager(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var OwnerEmail = User.Identity.GetUserName();
                string pwd = "@User001";
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, pwd);
                     model.Password = pwd;
                     model.ConfirmPassword = pwd;
                     Manager manager = new Manager();
                    manager.FullName = model.FullName;
                    manager.LastName = model.LastName;
                    manager.Email = model.Email;
                    manager.Gender = model.Gender;
                    manager.Nationality = "South African";
                    manager.AltContactNumber = model.AltContactNumber;
                    manager.Phone = model.Phone;
                    manager.OwnerEmail = OwnerEmail;
                    db.Managers.Add(manager);
                    db.SaveChanges();

                var provider = new DpapiDataProtectionProvider("SampleAppName");
                UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    provider.Create("SampleTokenName"));

                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Users", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(model.Email, model.FullName));
                var body = $"Hello {model.FullName}, Alliance Properties SA Team have created an account on your behalf as a Manager. <br/><br/>Your login credentials are as follows<br/><br/>" +
                    "Email (Username) : " + model.Email +
                    "<br/>Password : " + ConfigurationManager.AppSettings.Get("tempPassword").ToString() +
                    "<br/><br/> " +
                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";

                Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
                emailService.SendEmail(new EmailContent()
                {
                    mailTo = mailTo,
                    mailCc = new List<MailAddress>(),
                    mailSubject = "Confirm your account",
                    mailBody = body,
                    mailFooter = "<br/> Many Thanks, <br/> <b>Homelink</b>",
                    mailPriority = MailPriority.High,
                    mailAttachments = new List<Attachment>()
                });
                UserManager.AddToRole(user.Id, "Manager");


                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //UserManager.AddToRole(user.Id, "Manager");
                    return RedirectToAction("Index", "Managers");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public byte[] ConvertToBytes(HttpPostedFileBase files)
        {

            BinaryReader reader = new BinaryReader(files.InputStream);
            return reader.ReadBytes(files.ContentLength);
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase files)
            {
    
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (model.Type == "Landlord")
                {
                    //if (files != null && files.ContentLength > 0)
                    //{
                    //    model.FileName = files.FileName;
                    //    string[] bits = model.FileName.Split('\\');
                    //    model.FileContent = ConvertToBytes(files);
                    //}

                    Owner owner = new Owner();

                    owner.FullName = model.FullName;
                    owner.LastName = model.LastName;
                    owner.Email = model.Email;
                    owner.IDNumber = model.IDNumber;
                    owner.Phone = model.Phone;
                    owner.Type = model.Type;
                    owner.AltContactNumber = model.AltContactNumber;
                    owner.Status = "Awaiting Approval";
                    owner.UserId = user.Id;
                    //owner.FileContent = model.FileContent;
                    owner.FileName = model.FileName;
                    db.owners.Add(owner);
                    db.SaveChanges();
                    UserManager.AddToRole(user.Id, "Landlord");


                    }
                    //else
                    //{
                //    Tenant tenant = new Tenant();
                //tenant.TenantId = user.Id;
                //tenant.FullName = model.FullName;
                //tenant.LastName = model.LastName;
                //tenant.Phone = model.Phone;
                //tenant.IDNumber = model.IDNumber;
                //tenant.Email = user.UserName;
                //tenant.AltContactNumber = model.AltContactNumber;
                //tenant.Gender = "Male";
                //db.Tenants.Add(tenant);
                //db.SaveChanges();
                //UserManager.AddToRole(user.Id, "Tenant");
                //}

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //UserManager.AddToRole(user.Id, "Manager");
                    if (model.Type == "Tenant")
                    {
                    return RedirectToAction("tenant", "Home");

                    }
                    else
                    {
                        return RedirectToAction("Thankyou", "Home");

                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterCleaner(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var OwnerEmail = User.Identity.GetUserName();
                var building = db.buildings.Where(x => x.OwnerEmail == OwnerEmail).Select(x => x.Address).FirstOrDefault();
                string pwd = "@User001";
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, pwd);
                model.Password = pwd;
                model.ConfirmPassword = pwd;
                Cleaners cleaners = new Cleaners();
                cleaners.FullName = model.FullName;
                cleaners.LastName = model.LastName;
                cleaners.Email = model.Email;
                cleaners.Gender = model.Gender;
                cleaners.Phone = model.Phone;
                cleaners.OwnerEmail = OwnerEmail;
                cleaners.buildingName = building;
                db.cleaners.Add(cleaners);
                db.SaveChanges();

                var provider = new DpapiDataProtectionProvider("SampleAppName");
                UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                    provider.Create("SampleTokenName"));

                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Users", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                var mailTo = new List<MailAddress>();
                mailTo.Add(new MailAddress(model.Email, model.FullName));
                var body = $"Hello {model.FullName}, Alliance Properties SA Team have created an account on your behalf as a Manager. <br/><br/>Your login credentials are as follows<br/><br/>" +
                    "Email (Username) : " + model.Email +
                    "<br/>Password : " + ConfigurationManager.AppSettings.Get("tempPassword").ToString() +
                    "<br/><br/> " +
                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";

                Accommodation.Services.Implementation.EmailService emailService = new Accommodation.Services.Implementation.EmailService();
                emailService.SendEmail(new EmailContent()
                {
                    mailTo = mailTo,
                    mailCc = new List<MailAddress>(),
                    mailSubject = "Confirm your account",
                    mailBody = body,
                    mailFooter = "<br/> Many Thanks, <br/> <b>Homelink</b>",
                    mailPriority = MailPriority.High,
                    mailAttachments = new List<Attachment>()
                });
                UserManager.AddToRole(user.Id, "Cleaner");

                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //UserManager.AddToRole(user.Id, "Manager");
                    return RedirectToAction("Index", "Cleaners");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult Download(int? id)
        {
            //var r = db.PDFs.Find(id);

            //return File(r.file, "application/pdf", r.location);

            //var r = db.PDFs.Find(id);
            //PDF pDF = db.PDFs.Find(id);
            //string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
            //embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            //embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            //embed += "</object>";
            //TempData["Embed"] = string.Format(embed, VirtualPathUtility.ToAbsolute(r.location).ToString());

            MemoryStream ms = null;

            var item = db.owners.Where(x => x.ownerID == id).FirstOrDefault();
            if (item != null)
            {
                ms = new MemoryStream(item.FileContent);
            }
            return new FileStreamResult(ms, item.FileName);
            //return RedirectToAction("Download");
        }



        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("tenant", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}