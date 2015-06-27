using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VkPostAnalyser.Services.Authentication
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {
        public ApplicationSignInManager(UserManager<ApplicationUser, int> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return ((UserManager<ApplicationUser, int>)UserManager).CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}