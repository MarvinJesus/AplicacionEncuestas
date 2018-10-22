using CoreApi.ActionResult;
using System.Configuration;
using System.IO;

namespace WebApi.Helper
{
    public class GetPictures
    {
        private string SERVER_PATH_FOLDER { get; set; }

        public GetPictures()
        {
            SERVER_PATH_FOLDER = ConfigurationManager.AppSettings["UPLOAD_DIR"];
        }

        public ManagerActionResult<FileStream> GetPicture(string imageId)
        {
            var route = System.Web.HttpContext.Current.Server.MapPath(SERVER_PATH_FOLDER);

            var routeCompleted = string.Format("{0}/{1}", route, imageId);

            if (!File.Exists(routeCompleted)) return new ManagerActionResult<FileStream>(null, ManagerActionStatus.NotFound);

            var fileStream = File.OpenRead(routeCompleted);

            return new ManagerActionResult<FileStream>(fileStream, ManagerActionStatus.Ok);
        }

        public ManagerActionResult<FileStream> DeletePicture(FileStream fileStream)
        {
            return new ManagerActionResult<FileStream>(null, ManagerActionStatus.Deleted);
        }
    }
}