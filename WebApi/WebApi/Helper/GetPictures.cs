using CoreApi.ActionResult;
using System.IO;

namespace WebApi.Helper
{
    public class GetPictures : Picture
    {
        public ManagerActionResult<FileStream> GetPicture(string fileName)
        {
            var routeCompleted = GetRouteCompleted(fileName);

            if (!File.Exists(routeCompleted)) return new ManagerActionResult<FileStream>(null, ManagerActionStatus.NotFound);

            var fileStream = File.OpenRead(routeCompleted);

            return new ManagerActionResult<FileStream>(fileStream, ManagerActionStatus.Ok);
        }
    }
}