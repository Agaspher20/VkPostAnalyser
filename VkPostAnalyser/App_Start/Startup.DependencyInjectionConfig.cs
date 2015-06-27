using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using VkPostAnalyser.Services;
using VkPostAnalyser.Services.Authentication;
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
            builder.Register<IOwinContext>((c, p) => HttpContext.Current.GetOwinContext()).InstancePerRequest();
            builder.Register<IAuthenticationManager>((c, p) => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();

            var dataProtectionProvider = app.GetDataProtectionProvider();

            builder.RegisterType<DataContext>().As<DbContext>().InstancePerRequest();
            builder.Register<DataContext>((c, p) => (DataContext)c.Resolve<DbContext>());
            builder.RegisterType<ApplicationSignInManager>().As<SignInManager<ApplicationUser, string>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.Register<UserManager<ApplicationUser>>((c, p) => BuildUserManager(c, p, dataProtectionProvider));
            builder.RegisterType<VkApiProvider>().As<ISocialApiProvider>().InstancePerRequest();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().As<SignInManager<ApplicationUser, string>>().InstancePerRequest();
        }

        private ApplicationUserManager BuildUserManager(IComponentContext context, IEnumerable<Parameter> parameters, IDataProtectionProvider dataProtectionProvider)
        {
            var manager = new ApplicationUserManager(context.Resolve<IUserStore<ApplicationUser>>());
            
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
