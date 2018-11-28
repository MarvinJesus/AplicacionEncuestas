using Entities_POJO;
using Microsoft.AspNet.Identity;
using SurveyOnline.Web.Services;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    [Authorize]
    public class SurveyOnlineController : Controller
    {
        public Guid GetUserId()
        {
            return Guid.Parse(User.Identity.GetUserId());
        }

        public string GetAccessToken()
        {
            var claims = User as ClaimsPrincipal;

            return claims.FindFirst("access_token").Value;
        }

        public void ProfileSession()
        {
            var claims = User as ClaimsPrincipal;
            var service = new ProfileService(claims.FindFirst("access_token").Value);

            Profile profile = service.GetProfile(Guid.Parse(User.Identity.GetUserId()));

            if (profile == null)
            {
                profile = GetDefaultUser();
            }

            Session["Username"] = profile.Name;
            Session["Picture"] = profile.ImagePath;
        }

        public Profile GetDefaultUser()
        {
            var imageName = "defaultProfilePicture.png";
            var path = ConfigurationManager.AppSettings["IMAGE_DIR"];
            var completeRoute = path + @"\" + imageName;

            return new Profile()
            {
                Name = "anonimo",
                ImagePath = completeRoute
            };
        }
    }
}