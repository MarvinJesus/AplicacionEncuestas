using Entities_POJO;
using Microsoft.AspNet.Identity;
using SurveyOnline.Web.Services;
using SurveyOnline.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public async Task<ActionResult> MyProfile()
        {
            Profile profile = null;

            var viewModel = new ProfileViewModel();

            var claims = User as ClaimsPrincipal;
            var accessToken = claims.FindFirst("access_token").Value;

            var services = new ProfileService(accessToken);
            var topicService = new TopicService(accessToken);

            var userId = Guid.Parse(User.Identity.GetUserId());
            var paramList = new Dictionary<string, string>
            {
                { "sort", "id" },
                { "fields","id,title,imagepath,totalSurvey"}
            };

            profile = await services.GetProfileAsync(userId);
            viewModel.Topics = await topicService.GetUserTopicsAsync(userId, paramList);

            if (profile == null) profile = new Profile();

            viewModel.Profile = profile;

            return View(viewModel);
        }

        public ActionResult ProfileSession()
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

            return RedirectToAction("Index", "Topic");
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

        [HttpPost]
        public async Task<ActionResult> ChangePicture(HttpPostedFileBase file)
        {
            var claims = User as ClaimsPrincipal;

            var service = new ProfileService(claims.FindFirst("access_token").Value);

            var profile = await service.ChangePictureAsync(file.InputStream, file.FileName,
                file.ContentLength, Guid.Parse(User.Identity.GetUserId()));

            if (profile != null) Session["Picture"] = profile.ImagePath;

            return RedirectToAction("MyProfile", "Profile");
        }

        [HttpPost]
        public async Task<ActionResult> EditUserInfo(Profile profile)
        {
            if (profile == null) return RedirectToAction("MyProfile", "Profile");
            var claims = User as ClaimsPrincipal;
            var services = new ProfileService(claims.FindFirst("access_token").Value);

            profile.UserId = Guid.Parse(User.Identity.GetUserId());

            var profileEdited = await services.EditProfileAsync(profile);

            if (profileEdited != null) Session["Username"] = profileEdited.Name;

            return RedirectToAction("MyProfile", "Profile");
        }
    }
}