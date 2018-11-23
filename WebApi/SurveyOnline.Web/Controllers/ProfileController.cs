using Entities_POJO;
using Microsoft.AspNet.Identity;
using SurveyOnline.Web.Services;
using SurveyOnline.Web.ViewModels;
using System;
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
            var viewModel = new ProfileViewModel();
            Profile profile = null;

            var claims = User as ClaimsPrincipal;
            var services = new ProfileService(claims.FindFirst("access_token").Value);

            profile = await services.GetProfileAsync(Guid.Parse(User.Identity.GetUserId()));

            if (profile == null) profile = new Profile();

            viewModel.Profile = profile;

            return View(viewModel);
        }

        public async Task<ActionResult> ProfileSession()
        {
            var claims = User as ClaimsPrincipal;
            var service = new ProfileService(claims.FindFirst("access_token").Value);

            var profile = await service.GetProfileAsync(Guid.Parse(User.Identity.GetUserId()));

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
    }
}