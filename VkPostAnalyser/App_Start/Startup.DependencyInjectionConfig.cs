using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VkPostAnalyser.Domain.Configuration;
using VkPostAnalyser.Domain.Model;
using VkPostAnalyser.Domain.Services;
using VkPostAnalyser.Domain.Services.VkApi;
using VkPostAnalyser.Services.Authentication;

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
            builder.Register<IOwinContext>((c, p) => HttpContext.Current.GetOwinContext()).InstancePerRequest();
            builder.Register<IAuthenticationManager>((c, p) => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();

            var dataProtectionProvider = app.GetDataProtectionProvider();

            builder.Register<IReportsQueueConnector>(BuildReportsQueueConnector).InstancePerRequest();
            builder.RegisterType<DataContext>().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().As<SignInManager<ApplicationUser, int>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser, int>>().InstancePerRequest();
            builder.Register<UserManager<ApplicationUser, int>>((c, p) => BuildUserManager(c, p, dataProtectionProvider));
            builder.RegisterType<VkApiProvider>().As<ISocialApiProvider>().InstancePerRequest();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().As<SignInManager<ApplicationUser, int>>().InstancePerRequest();
        }

        private IReportsQueueConnector BuildReportsQueueConnector(IComponentContext arg, IEnumerable<Parameter> parameters)
        {
            var serviceBusConfig = (ServiceBusConfiguration)ConfigurationManager.GetSection(ServiceBusConfiguration.SectionName);
            return new ReportsQueueConnector(serviceBusConfig);
        }

        private ApplicationUserManager BuildUserManager(IComponentContext context, IEnumerable<Parameter> parameters, IDataProtectionProvider dataProtectionProvider)
        {
            var manager = new ApplicationUserManager(context.Resolve<IUserStore<ApplicationUser, int>>());
            
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
