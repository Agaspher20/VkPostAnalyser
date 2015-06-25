using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using VkPostAnalyser.Services;
using VkPostAnalyser.Services.VkApi;

namespace VkPostAnalyser
{
    public partial class Startup
    {
        private void ConfigureDependencyInjection(IAppBuilder app, HttpConfiguration apiConfig)
        {
            var builder = new ContainerBuilder();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(executingAssembly);
            builder.RegisterControllers(executingAssembly);

            RegisterComponents(builder, app);

            var container = builder.Build();

            app.UseAutofacMiddleware(container);

            var apiResolver = new AutofacWebApiDependencyResolver(container);
            apiConfig.DependencyResolver = apiResolver;
            app.UseAutofacWebApi(apiConfig);

            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
            app.UseAutofacMvc();
        }

        private void RegisterComponents(ContainerBuilder builder, IAppBuilder app)
        {
            builder.RegisterType<EfRepository>().As<IRepository>().InstancePerRequest();
            builder.RegisterType<VkApiProvider>().As<ISocialApiProvider>().InstancePerRequest();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerRequest();
        }
    }
}
