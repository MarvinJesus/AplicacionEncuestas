using CoreApi;
using DataAccess.Factory;
using Entities_POJO;
using Exceptions;
using System;
using System.Linq;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class ProfilesController : SurveyOnlineController
    {
        private IProfileManager _manager { get; set; }

        public ProfilesController(IProfileManager ProfileManager, IUserManger userManger)
        {
            _manager = ProfileManager;
        }

        [HttpGet]
        //[ScopeAuthorize("read")]
        public IHttpActionResult GetProfile(Guid userId, string fields = null)
        {
            try
            {
                //if (!userId.Equals(GetProfileId())) return Unauthorized();

                var profile = _manager.GetProfile(new Profile { UserId = userId });
                var factory = new ProfileFactory();

                if (profile == null)
                    return NotFound();

                profile.ImagePath = string.Format("{0}/pictures", Request.RequestUri);

                if (fields != null)
                {
                    var fieldsList = fields.Split(',').ToList();
                    return Ok(factory.CreateDataShapeObject(profile, fieldsList));
                }
                else
                {
                    return Ok(factory.CreateDataShapeObject(profile));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }
    }
}
