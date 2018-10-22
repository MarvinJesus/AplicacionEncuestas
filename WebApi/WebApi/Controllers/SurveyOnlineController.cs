using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class SurveyOnlineController : ApiController
    {

        public Guid GetProfileId()
        {
            return new Guid("d69466ae-e0d9-446a-8ec1-d22f2c9afe8e");
        }
    }
}
