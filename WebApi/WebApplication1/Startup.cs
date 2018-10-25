using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;

[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var factory = new IdentityServerServiceFactory();
            factory.UseInMemoryUsers(InMemoryManager.GetUser());
            factory.UseInMemoryScopes(InMemoryManager.GetScopes());
            factory.UseInMemoryClients(InMemoryManager.GetClients());

            var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings["SignedCertificate"]);
            var options = new IdentityServerOptions()
            {
                SigningCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificate,
                ConfigurationManager.AppSettings["SignedCertificatePassword"]),
                RequireSsl = false,
                Factory = factory
            };

            app.UseIdentityServer(options);
        }

        public class InMemoryManager
        {
            public static List<InMemoryUser> GetUser()
            {
                return new List<InMemoryUser>
                {
                    new InMemoryUser
                    {
                        Subject = "adrian10596@live.com",
                        Username = "adrian10596@live.com",
                        Password = "password",
                        Claims = new []
                        {
                            new Claim(Constants.ClaimTypes.Name,"Adrian Vega")
                        }
                    }
                };
            }

            public static IEnumerable<Scope> GetScopes()
            {
                return new[] {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.OfflineAccess,
                    new Scope
                    {
                        DisplayName = "Read to user data",
                        Name = "read"
                    }
                };
            }

            public static IEnumerable<Client> GetClients()
            {
                return new[]
                {
                    new Client
                    {
                        ClientId = "survey_online",
                        ClientSecrets = new List<Secret>
                        {
                            new Secret("secret".Sha256())
                        },
                        ClientName = "SurverOnline",
                        Flow = Flows.ResourceOwner,
                        AllowedScopes = new List<string>
                        {
                            Constants.StandardScopes.OpenId,
                            "read"
                        },
                        Enabled = true
                    }
                };
            }
        }
    }
}

