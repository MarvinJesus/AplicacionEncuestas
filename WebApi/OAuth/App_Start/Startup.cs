using CoreApi;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using Microsoft.Owin;
using OAuth.Config;
using Owin;
using System;
using System.Configuration;

[assembly: OwinStartup(typeof(OAuth.App_Start.Startup))]

namespace OAuth.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var inMemoryManager = new Config.Startup.InMemoryManager();

            var factory = new IdentityServerServiceFactory();
            factory.UseInMemoryScopes(inMemoryManager.GetScopes());
            factory.UseInMemoryClients(inMemoryManager.GetClients());

            factory.UserService = new Registration<IUserService>(typeof(SurveyOnlineUserServices));
            factory.Register(new Registration<IProfileManager>(new ProfileManager()));
            factory.Register(new Registration<IUserClaimManager>(new UserClaimManager()));

            var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings["SignedCertificate"]);
            var options = new IdentityServerOptions()
            {
                SiteName = "SurverOnline",
                SigningCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificate,
                ConfigurationManager.AppSettings["SignedCertificatePassword"]),
                RequireSsl = false,
                Factory = factory
            };

            app.UseIdentityServer(options);
        }
    }
}
