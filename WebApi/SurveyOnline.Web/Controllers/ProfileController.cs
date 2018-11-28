using Entities_POJO;
using SurveyOnline.Web.Services;
using SurveyOnline.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    [Authorize]
    public class ProfileController : SurveyOnlineController
    {
        public async Task<ActionResult> MyProfile()
        {
            Profile profile = null;

            var viewModel = new ProfileViewModel();
            var accessToken = GetAccessToken();

            var services = new ProfileService(accessToken);
            var topicService = new TopicService(accessToken);

            var userId = GetUserId();
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

        [HttpPost]
        public async Task<ActionResult> ChangePicture(HttpPostedFileBase file)
        {
            var service = new ProfileService(GetAccessToken());

            var profile = await service.ChangePictureAsync(file.InputStream, file.FileName,
                file.ContentLength, GetUserId());

            if (profile != null) Session["Picture"] = profile.ImagePath;

            return RedirectToAction("MyProfile", "Profile");
        }

        [HttpPost]
        public async Task<ActionResult> EditUserInfo(Profile profile)
        {
            if (profile == null) return RedirectToAction("MyProfile", "Profile");
            var services = new ProfileService(GetAccessToken());

            profile.UserId = GetUserId();

            var profileEdited = await services.EditProfileAsync(profile);

            if (profileEdited != null) Session["Username"] = profileEdited.Name;

            return RedirectToAction("MyProfile", "Profile");
        }

        public ActionResult UserInfo()
        {
            base.ProfileSession();

            return RedirectToAction("Index", "Topic");
        }
    }
}