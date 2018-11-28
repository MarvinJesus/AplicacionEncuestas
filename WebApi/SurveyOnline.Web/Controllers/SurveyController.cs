using Entities_POJO;
using SurveyOnline.Web.Services;
using SurveyOnline.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    [Authorize]
    public class SurveyController : SurveyOnlineController
    {
        public async Task<ActionResult> SurveyForm()
        {
            var topicService = new TopicService(GetAccessToken());

            var paramList = new Dictionary<string, string>
            {
                { "sort", "id" },
                { "fields","id,title"}
            };

            var model = new SurveyFormViewModel
            {
                Topics = await topicService.GetUserTopicsAsync(GetUserId(), paramList)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterSurvey(Survey survey)
        {
            var service = new SurveyService(GetAccessToken());

            var newSurvey = await service.RegisterSurveyAsync(survey);

            if (newSurvey == null)
            {
                return Content("La encuesta no pudo ser registrada");
            }

            return Json(Url.Action("Index", "Topic"));
        }
    }
}
