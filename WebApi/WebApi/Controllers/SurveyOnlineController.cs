using System;
using System.Security.Claims;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class SurveyOnlineController : ApiController
    {
        public Guid GetProfileId()
        {
            var user = User as ClaimsPrincipal;
            var userId = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            return new Guid(userId);
        }
    }
}
