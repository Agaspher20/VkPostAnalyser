using Microsoft.Owin;
using Owin;

namespace VkPostAnalyser
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var apiConfig = ConfigureWebApi();
            ConfigureDependencyInjection(app, apiConfig);
            ConfigureAuth(app);
            app.UseWebApi(apiConfig);
        }
    }
}
