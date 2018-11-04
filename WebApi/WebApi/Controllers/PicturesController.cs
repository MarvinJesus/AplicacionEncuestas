using CoreApi;
using CoreApi.ActionResult;
using Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;
using WebApi.Helper;

namespace WebApi.Controllers
{

    public class PicturesController : SurveyOnlineController
    {
        private IProfileManager _profileManager { get; set; }
        private IList<string> AllowedFilesExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".gif" };
        private readonly int MaxContentLength = 39072;

        public PicturesController(IProfileManager profileManager)
        {
            _profileManager = profileManager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("pictures")]
        public IHttpActionResult GetPicture(string pictureName)
        {
            try
            {
                var result = new GetPictures().GetPicture(pictureName);

                if (result.Status == ManagerActionStatus.Ok)
                {
                    var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StreamContent(result.Entity)
                    };
                    responseMessage.Content.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(
                            string.Concat("image/", Path.GetExtension(result.Entity.Name).Substring(1)));

                    return ResponseMessage(responseMessage);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }



        [HttpPost]
        [ScopeAuthorize("write")]
        [Route("profiles/{profileId}/pictures")]
        public IHttpActionResult PostPicture(Guid profileId)
        {
            try
            {
                if (!profileId.Equals(GetProfileId())) return Unauthorized();

                var profile = _profileManager.GetProfile(new Entities_POJO.Profile { UserId = profileId });

                if (profile == null) return NotFound();

                if (!Request.Content.IsMimeMultipartContent()) return StatusCode(System.Net.HttpStatusCode.UnsupportedMediaType);

                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string filename in httpRequest.Files)
                    {
                        var file = httpRequest.Files[filename];

                        if (file.ContentLength > 0 && file.ContentLength <= MaxContentLength)
                        {
                            if (AllowedFilesExtensions.Contains(Path.GetExtension(file.FileName)))
                            {
                                var result = new CreatePictures().CreatePicture(profile, file);

                                if (result.Status == ManagerActionStatus.Created)
                                {
                                    result.Entity.ImagePath = string.Concat(PathForPicture.GetInstance().GetPicturePath(Request.RequestUri.PathAndQuery,
                                        Request.RequestUri.AbsoluteUri), result.Entity.ImagePath);

                                    return Created(Request.RequestUri, result.Entity);
                                }
                            }
                            else
                            {
                                return BadRequest("File extension is not allowed");
                            }
                        }
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }
    }
}
