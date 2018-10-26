using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    [Authorize]
    public class ProfilesController : SurveyOnlineController
    {
        private IProfileManager _manager { get; set; }

        public ProfilesController(IProfileManager ProfileManager, IUserManger userManger)
        {
            _manager = ProfileManager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        public IHttpActionResult GetProfile(Guid userId)
        {
            try
            {
                if (!userId.Equals(GetProfileId())) return Unauthorized();

                var profile = _manager.GetProfile(new Profile { UserId = userId });

                if (profile == null)
                    return NotFound();

                profile.ImagePath = string.Format("{0}/pictures", Request.RequestUri); ;

                return Ok(profile);
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }
    }
}
