using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using SurveyOnline.Constants;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(SurveyOnline.Web.App_Start.Startup))]

namespace SurveyOnline.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var jwt = new JwtSecurityTokenHandler
            {
                InboundClaimTypeMap = new Dictionary<string, string>()
            };



            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "surveyOnline_implicit",
                Authority = SurveyOnlineConstants.SurveyOnlineOAuth,
                RedirectUri = SurveyOnlineConstants.SurveyOnlineClient,
                ResponseType = "token id_token",
                Scope = "openid profile read write roles",
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                PostLogoutRedirectUri = SurveyOnlineConstants.SurveyOnlineClientLogout,
                RequireHttpsMetadata = false,

                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    SecurityTokenValidated = notification =>
                    {
                        var identity = notification.AuthenticationTicket.Identity;

                        identity.AddClaim(new Claim("id_token",
                            notification.ProtocolMessage.IdToken));

                        identity.AddClaim(new Claim("access_token",
                            notification.ProtocolMessage.AccessToken));

                        notification.AuthenticationTicket =
                        new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    },

                    RedirectToIdentityProvider = notification =>
                    {
                        if (notification.ProtocolMessage.RequestType != OpenIdConnectRequestType.Logout)
                        {
                            return Task.FromResult(0);
                        }

                        notification.ProtocolMessage.IdTokenHint =
                        notification.OwinContext.Authentication.User.FindFirst("id_token").Value;

                        return Task.FromResult(0);
                    }
                },
            });
        }
    }
}
