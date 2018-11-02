using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using SurveyOnline.Constants;
using System.Reflection;
using System.Web.Http;
using WebApi.Autofac.Modules;

[assembly: OwinStartup(typeof(WebApi.App_Start.Startup))]

namespace WebApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            var container = new ContainerBuilder();

            container.RegisterApiControllers(Assembly.GetExecutingAssembly());
            container.RegisterModule<ModulosEncuesta>();

            var build = container.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(build);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServer3.AccessTokenValidation.IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = SurveyOnlineConstants.SurveyOnlineOAuth,
            });
        }
    }
}
