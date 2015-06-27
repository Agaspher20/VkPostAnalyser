using System.Web.Http;

namespace VkPostAnalyser
{
    public partial class Startup
    {
        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}");
            return config;
        }
    }
}
