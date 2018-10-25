using CoreApi;
using CoreApi.ActionResult;
using Entities_POJO;
using Exceptions;
using System;
using System.Security.Claims;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class ProfilesController : ApiController
    {
        private IProfileManager _manager { get; set; }
        private IUserManger _userManager { get; set; }

        public ProfilesController(IProfileManager ProfileManager, IUserManger userManger)
        {
            _manager = ProfileManager;
            _userManager = userManger;
        }

        [HttpGet]
        public IHttpActionResult GetProfile(string username, string password)
        {
            var profile = _manager.GetProfile(username, password);

            if (profile == null) return NotFound();

            return Ok(profile);
        }

        [HttpGet]
        [Authorize]
        public IHttpActionResult GetProfile(Guid userId)
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var claims = claimsPrincipal.Claims;
            var id = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

            var profile = _manager.GetProfile(new Profile { UserId = userId });

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpGet]
        public IHttpActionResult GetProfile()
        {
            var userId = 2;

            return Ok(userId);
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult PostProfile([FromBody]ProfileForRegistration profileForRegistration)
        {
            try
            {
                if (profileForRegistration == null)
                    return BadRequest();

                var result = _manager.RegisterProfile(profileForRegistration);

                if (result.Status == ManagerActionStatus.Created)
                {
                    result.Entity.ImagePath = string.Format("{0}/{1}/pictures", Request.RequestUri, result.Entity.UserId);

                    return Created<Profile>
                        (Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);
                }

                if (result.Status == ManagerActionStatus.Error && result.Exception != null)
                {
                    return BadRequest(result.Exception.AppMessage?.Message);
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public IHttpActionResult PostProfile([FromBody]ProfileForRegistration profileForRegistration)
        //{
        //    try
        //    {
        //        if (profileForRegistration == null)
        //            return BadRequest();

        //        var user = new User
        //        {
        //            Username = profileForRegistration.Email,
        //            Salt = Cryptographic.GenerateSalt()
        //        };
        //        user.Password = Cryptographic.HashPasswordWithSalt(Encoding.UTF8.GetBytes(profileForRegistration.Password), user.Salt);

        //        var actionResult = _userManager.RegisterUser(user);

        //        if (actionResult.Status == ManagerActionStatus.Created)
        //        {
        //            var profile = ProfileFactory.CreateProfile(profileForRegistration);
        //            profile.UserId = actionResult.Entity.UserId;

        //            var result = _manager.RegisterProfile(profile);

        //            if (result.Status == ManagerActionStatus.Created)
        //            {
        //                return Created<Profile>
        //                    (Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);
        //            }
        //            else
        //            {
        //                if (actionResult.Status == ManagerActionStatus.Error)
        //                {
        //                    return BadRequest(actionResult.Exception?.AppMessage?.Message);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (actionResult.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
        //            {
        //                return BadRequest(actionResult.Exception?.AppMessage?.Message);
        //            }
        //        }

        //        return BadRequest();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        ExceptionManager.GetInstance().Process(ex);
        //        return InternalServerError();
        //    }
        //}
    }
}
