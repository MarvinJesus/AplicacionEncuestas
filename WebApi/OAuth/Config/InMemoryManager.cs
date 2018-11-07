using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using SurveyOnline.Constants;
using System.Collections.Generic;

namespace OAuth.Config
{
    public partial class Startup
    {
        public class InMemoryManager
        {
            //public List<InMemoryUser> GetUser()
            //{
            //    return new List<InMemoryUser>
            //    {
            //        new InMemoryUser()
            //        {
            //            Subject = "b97facda-f26e-4aee-b97d-0ad074018e70",
            //            Username = "adrian10596@live.com",
            //            Password = "password",
            //            Claims = new []
            //            {
            //                new Claim(Constants.ClaimTypes.Name,"Adrian Vega"),
            //                new Claim(Constants.ClaimTypes.Role, "Admin"),
            //                new Claim(Constants.ClaimTypes.Role, "Manager")
            //            }
            //        }
            //    };
            //}

            public IEnumerable<Scope> GetScopes()
            {
                return new[] {
                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.Roles,
                    StandardScopes.OfflineAccess,
                    new Scope
                    {
                        Name = "roles",
                        DisplayName = "Role(s)",
                        Description = "Allow the application to see your role(s).",
                        Type = ScopeType.Identity,
                        Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim(Constants.ClaimTypes.Role,true),
                        }
                    },
                    new Scope
                    {
                        DisplayName = "Read to user data",
                        Name = "read"
                    },
                    new Scope
                    {
                        DisplayName = "Write to user data",
                        Name = "write"
                    }
                };
            }

            public IEnumerable<Client> GetClients()
            {
                return new[]
                {
                    new Client
                    {
                        ClientId = "surveyOnline_resourceOwner",
                        ClientSecrets = new List<Secret>
                        {
                            new Secret("E3090F57-D45A-4B95-9420-ED5C62B61725".Sha256())
                        },
                        ClientName = "Survey online resource owner",
                        Flow = Flows.ResourceOwner,
                        AllowedScopes = new List<string>
                        {
                            Constants.StandardScopes.OpenId,
                            Constants.StandardScopes.Profile,
                            "read",
                            "write",
                            "roles"
                        },
                        Enabled = true
                    },
                    new Client
                    {
                        ClientId = "surveyOnline_implicit",
                        ClientSecrets = new List<Secret>
                        {
                            new Secret("E3090F57-D45A-4B95-9420-ED5C62B61725".Sha256())
                        },
                        ClientName = "Survey online implicit",
                        Flow = Flows.Implicit,
                        AllowedScopes = new List<string>
                        {
                            Constants.StandardScopes.OpenId,
                            Constants.StandardScopes.OfflineAccess,
                            Constants.StandardScopes.Profile,
                            "read",
                            "write",
                            "roles"
                        },
                        RedirectUris = new List<string>
                        {
                           SurveyOnlineConstants.SurveyOnlineClient,
                        },
                        PostLogoutRedirectUris = new List<string>
                        {
                           SurveyOnlineConstants.SurveyOnlineClientLogout,
                        },
                        Enabled = true
                    }
                };
            }
        }
    }
}
