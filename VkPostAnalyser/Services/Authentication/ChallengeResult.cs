using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace VkPostAnalyser.Services.Authentication
{
    internal class ChallengeResult : HttpUnauthorizedResult
    {
        private const string XsrfKey = "C871C8232AF443BF8B750EC27CE89CB5";
        private readonly IAuthenticationManager _authenticationManager;

        public ChallengeResult(string provider, string redirectUri, IAuthenticationManager authenticationManager)
            : this(provider, redirectUri, null, authenticationManager)
        {
        }

        public ChallengeResult(string provider, string redirectUri, string userId, IAuthenticationManager authenticationManager)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
            _authenticationManager = authenticationManager;
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
            _authenticationManager.Challenge(properties, LoginProvider);
        }
    }
}
