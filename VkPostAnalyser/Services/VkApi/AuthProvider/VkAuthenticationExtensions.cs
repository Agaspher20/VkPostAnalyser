using Owin;
using System;
using VKSharp.Data.Api;

namespace VkPostAnalyser.Services.VkApi.AuthProvider
{
    public static class VkAuthenticationExtensions
    {
        public static IAppBuilder UseVkontakteAuthentication(this IAppBuilder app, VkAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(VkAuthenticationMiddleware), app, options);
            return app;
        }

        public static IAppBuilder UseVkontakteAuthentication(this IAppBuilder app, int clientId, string clientSecret, VKPermission permissions)
        {
            return UseVkontakteAuthentication(app, new VkAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Permissions = permissions
            });
        }
    }
}
