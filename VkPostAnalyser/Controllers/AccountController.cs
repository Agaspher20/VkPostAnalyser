using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VkPostAnalyser.Domain.Model;
using VkPostAnalyser.Model;
using VkPostAnalyser.Services.Authentication;

namespace VkPostAnalyser.Controllers
{
    [Authorize]
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly SignInManager<ApplicationUser, int> _signInManager;
        public AccountController(IAuthenticationManager authenticationManager,
            SignInManager<ApplicationUser, int> signInManager)
        {
            _authenticationManager = authenticationManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View(new ExternalLoginListViewModel
            {
                ReturnUrl = returnUrl,
                LoginProviders = _authenticationManager.GetExternalAuthenticationTypes().ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }), _authenticationManager);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            return RedirectToLocal(returnUrl);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
