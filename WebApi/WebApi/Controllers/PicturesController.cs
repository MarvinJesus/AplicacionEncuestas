using CoreApi;
using Exceptions;
using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using WebApi.Helper;

namespace WebApi.Controllers
{
    public class PicturesController : SurveyOnlineController
    {
        private IProfileManager _profileManager { get; set; }

        public PicturesController(IProfileManager profileManager)
        {
            _profileManager = profileManager;
        }

        [HttpGet]
        [Route("api/profiles/{profileId}/pictures")]
        public IHttpActionResult GetImages(Guid? profileId = null)
        {
            try
            {
                if (profileId == null) return BadRequest("Invalid profile Id");

                if (!profileId.Equals(GetProfileId())) return Unauthorized();

                var profile = _profileManager.GetProfile(new Entities_POJO.Profile { UserId = (Guid)profileId });

                if (profile == null) return NotFound();

                var result = new GetPictures().GetPicture(profile.ImagePath);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Ok)
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

        //[HttpPost]
        //[Route("api/profile/{profileI}/picture")]
        //public IHttpActionResult PostPicture(Guid profileId)
        //{
        //    try
        //    {
        //        if (profileId == null) return BadRequest();

        //        if (profileId.Equals(GetProfileId())) return Unauthorized();

        //        return Ok();

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.GetInstance().Process(ex);
        //        return InternalServerError();
        //    }
        //}

    }
}
