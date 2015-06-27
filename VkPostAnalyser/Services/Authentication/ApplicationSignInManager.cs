using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using VkPostAnalyser.Services.VkApi.AuthProvider;

namespace VkPostAnalyser.Services.Authentication
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {
        public ApplicationSignInManager(UserManager<ApplicationUser, int> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {}

        public async override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            var identity = await ((UserManager<ApplicationUser, int>)UserManager).CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(VkConstants.ClaimTypes.Token, user.Token, "http://www.w3.org/2001/XMLSchema#string", DefaultAuthenticationTypes.ApplicationCookie));
            return identity;
        }
    }
}
