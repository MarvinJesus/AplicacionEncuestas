using WebApi.Controllers;

namespace WebApi.Helper
{
    public class PathForPicture
    {
        private string PictureControllerName { get; set; }

        private static PathForPicture _instance { get; set; }

        private PathForPicture()
        {
            PictureControllerName = nameof(PicturesController).Replace("Controller", "").ToLower();
        }

        public static PathForPicture GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PathForPicture();
            }

            return _instance;
        }

        public string GetPicturePath(string pathAndQuery, string absoluteUri)
        {
            return absoluteUri.Replace(pathAndQuery,
                string.Format("/{0}?pictureName=", PictureControllerName));
        }
    }
}