using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace WebApi.Helper
{
    public class Picture
    {
        private string SERVER_PATH_FOLDER { get; set; }

        public Picture()
        {
            SERVER_PATH_FOLDER = ConfigurationManager.AppSettings["UPLOAD_DIR"];
        }

        public string GetRouteCompleted(string pictureName)
        {
            var route = HttpContext.Current.Server.MapPath(SERVER_PATH_FOLDER);
            var routeCompleted = string.Concat(route, "/", pictureName);

            return routeCompleted;
        }

        public string GetNewPictureName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var pictureName = string.Format("{0}{1}{2}", Guid.NewGuid().ToString(),
                DateTime.Today.ToString("dd-MM-yyyy"), extension);

            return pictureName;
        }
    }
}