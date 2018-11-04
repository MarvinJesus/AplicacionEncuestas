using CoreApi;
using CoreApi.ActionResult;
using Entities_POJO;
using System.IO;
using System.Web;

namespace WebApi.Helper
{
    public class CreatePictures : Picture
    {
        private IProfileManager _profileManger { get; set; }

        public CreatePictures()
        {
            _profileManger = new ProfileManager();
        }

        public string CreatePicture(PictureForEntity picture)
        {
            if (picture == null && picture.Picture == null) return string.Empty;

            var filename = GetNewPictureName(picture.Extension);

            File.WriteAllBytes(GetRouteCompleted(filename), picture.Picture);

            return filename;
        }

        public ManagerActionResult<Profile> CreatePicture(Profile profile, HttpPostedFile fileStream)
        {
            var pictureName = GetNewPictureName(fileStream.FileName);

            fileStream.SaveAs(GetRouteCompleted(pictureName));
            profile.ImagePath = pictureName;

            return _profileManger.EditPicture(profile);
        }
    }
}