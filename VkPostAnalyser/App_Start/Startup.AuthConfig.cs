using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Linq;
using System.Configuration;
using VkPostAnalyser.Misc;
using VkPostAnalyser.Services.Authentication;
using VkPostAnalyser.Services.VkApi.AuthProvider;

namespace VkPostAnalyser
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            var authConfig = (VkConfiguration)ConfigurationManager.GetSection(VkConfiguration.SectionName);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager<ApplicationUser, int>, ApplicationUser, int>(
                        TimeSpan.FromMinutes(30),
                        (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                        ci =>
                        {
                            var idClaim = ci.Claims.First(c => c.Type == VkConstants.ClaimTypes.Id);
                            return int.Parse(idClaim.Value);
                        })
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseVkontakteAuthentication(authConfig.ApplicationId, authConfig.ApplicationKey, authConfig.ApplicationPermissions);
        }
    }
}
