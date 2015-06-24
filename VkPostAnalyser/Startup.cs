using Microsoft.Owin;
using Owin;

namespace VkPostAnalyser
{
    [assembly: OwinStartupAttribute(typeof(VkPostAnalyser.Startup))]
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var apiConfig = ConfigureWebApi();
            ConfigureDependencyInjection(app, apiConfig);
            app.UseWebApi(apiConfig);
        }
    }
}
