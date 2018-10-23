using CoreApi;
using CoreApi.ActionResult;
using Entities_POJO;
using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace WebApi.Helper
{
    public class CreatePictures
    {
        private string SERVER_PATH_FOLDER { get; set; }
        private IProfileManager _profileManger { get; set; }

        public CreatePictures()
        {
            SERVER_PATH_FOLDER = ConfigurationManager.AppSettings["UPLOAD_DIR"];
            _profileManger = new ProfileManager();
        }

        public ManagerActionResult<Profile> CreatePicture(Profile profile, HttpPostedFile fileStream)
        {
            var route = HttpContext.Current.Server.MapPath(SERVER_PATH_FOLDER);
            var extension = Path.GetExtension(fileStream.FileName);
            var pictureName = string.Format("{0}{1}{2}", Guid.NewGuid().ToString(), DateTime.Today.ToString("dd-MM-yyyy"), extension);
            var routeCompleted = string.Concat(route, "/", pictureName);

            fileStream.SaveAs(routeCompleted);

            profile.ImagePath = pictureName;

            return _profileManger.EditPicture(profile);
        }
    }
}