using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class SurveyOnlineController : ApiController
    {
        public Guid GetProfileId()
        {
            return new Guid("B97FACDA-F26E-4AEE-B97D-0AD074018E70");
        }
    }
}
