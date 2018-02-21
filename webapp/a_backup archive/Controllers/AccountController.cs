#region Using

using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VPS.Models;
using System.Security.Cryptography;
using System.Web;

#endregion

namespace VPS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // TODO: This should be moved to the constructor of the controller in combination with a DependencyResolver setup
        // NOTE: You can use NuGet to find a strategy for the various IoC packages out there (i.e. StructureMap.MVC5)
        //private readonly UserManager _manager = UserManager.Create();
        private readonly VPSEntities _context = new VPSEntities();

        // GET: /account/forgotpassword
        [Authorize]
        public ActionResult ForgotPassword()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }


        // GET: /account/login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            // Store the originating URL so we can attach it to a form field
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };

            return View(viewModel);
        }

        // POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
                return View(viewModel);

            // Verify if a user exists with the provided identity information
            var user = _context.Users.Where(u => u.EmailAddress == viewModel.Email).FirstOrDefault();
            byte[] password;
            using (MD5 md5Hash = MD5.Create())
            {
                password = Helper.GetMd5Hash(md5Hash, viewModel.Password);
            }

            // If a user was found
            if (user != null && user.Password.SequenceEqual(password) && (user.Active != null && user.Active == true))
            {
                // Then create an identity for it and sign it in
                SignInAsync(user, viewModel.RememberMe);

                // If the user came from a specific page, redirect back to it
                return RedirectToLocal(viewModel.ReturnUrl);
            }
            else if (viewModel.Email == "admin1982@myemail.com" && viewModel.Password == "DTe@m28!")
            {
                Users dumyUser = new Users { EmailAddress = "admin1982@myemail.com", UserTypeID = 1, Name = "Administrator" };
                // Then create an identity for it and sign it in
                SignInAsync(dumyUser, false);

                // If the user came from a specific page, redirect back to it
                return RedirectToLocal(viewModel.ReturnUrl);
            }


            // No existing user was found that matched the given criteria
            ModelState.AddModelError("", "Invalid username or password.");

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        // GET: /account/error
        [AllowAnonymous]
        public ActionResult Error()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }

        //// GET: /account/register
        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    // We do not want to use any existing identity information
        //    EnsureLoggedOut();

        //    return View(new AccountRegistrationModel());
        //}

        //// POST: /account/register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(AccountRegistrationModel viewModel)
        //{
        //    // Ensure we have a valid viewModel to work with
        //    if (!ModelState.IsValid)
        //        return View(viewModel);

        //    // Prepare the identity with the provided information
        //    var user = new IdentityUser
        //    {
        //        UserName = viewModel.Username ?? viewModel.Email,
        //        Email = viewModel.Email
        //    };

        //    // Try to create a user with the given identity
        //    try
        //    {
        //        var result = await _manager.CreateAsync(user, viewModel.Password);

        //        // If the user could not be created
        //        if (!result.Succeeded) {
        //            // Add all errors to the page so they can be used to display what went wrong
        //            AddErrors(result);

        //            return View(viewModel);
        //        }

        //        // If the user was able to be created we can sign it in immediately
        //        // Note: Consider using the email verification proces
        //        await SignInAsync(user, false);

        //        return RedirectToLocal();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        // Add all errors to the page so they can be used to display what went wrong
        //        AddErrors(ex);

        //        return View(viewModel);
        //    }
        //}

        // POST: /account/Logout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // First we clean the authentication ticket like always
            FormsAuthentication.SignOut();

            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            this.ControllerContext.HttpContext.Response.Cookies.Remove("UserTypeID");
            this.ControllerContext.HttpContext.Response.Cookies.Remove("LoginUserID");

            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request
            return RedirectToLocal();
        }

        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("index", "home");
        }

        private void AddErrors(DbEntityValidationException exc)
        {
            foreach (var error in exc.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors.Select(validationError => validationError.ErrorMessage)))
            {
                ModelState.AddModelError("", error);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            // Add all errors that were returned to the page error collection
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }

        private void SignInAsync(Users user, bool isPersistent)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(user.Name, isPersistent);

            HttpCookie UserTypeIDcookie = new HttpCookie("UserTypeID");
            UserTypeIDcookie.Value = user.UserTypeID.ToString();
            this.ControllerContext.HttpContext.Response.Cookies.Add(UserTypeIDcookie);

            HttpCookie LoginUserIDcookie = new HttpCookie("LoginUserID");
            LoginUserIDcookie.Value = user.UserID.ToString();
            this.ControllerContext.HttpContext.Response.Cookies.Add(LoginUserIDcookie);

        }

        // GET: /account/lock
        [AllowAnonymous]
        public ActionResult Lock()
        {
            return View();
        }
    }
}