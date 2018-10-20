using CoreApi;
using DataAccess.Factories;
using Dtos;
using Exceptions;
using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class ProfilesController : ApiController
    {
        private IProfileManager _manager { get; set; }

        public ProfilesController(IProfileManager ProfileManager)
        {
            _manager = ProfileManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult PostProfile([FromBody]ProfileDto ProfileDto)
        {
            try
            {
                if (ProfileDto == null)
                    return BadRequest();

                var result = _manager.RegisterProfile(ProfileDto);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                {
                    return Created<ProfileDto>
                        (Request.RequestUri + "/" + result.Entity.Id.ToString(), ProfileFactory.CreateProfile(result.Entity));
                }

                if (result.Exception != null)
                {
                    if (result.Exception.Code == 1)
                    {
                        return InternalServerError();
                    }
                    else
                    {
                        return BadRequest(result.Exception.AppMessage.Message);
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
