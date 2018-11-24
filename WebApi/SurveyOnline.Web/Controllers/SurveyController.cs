using Entities_POJO;
using Microsoft.AspNet.Identity;
using SurveyOnline.Web.Services;
using SurveyOnline.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyOnline.Web.Controllers
{
    public class SurveyController : Controller
    {
        public async Task<ActionResult> SurveyForm()
        {
            var userId = User.Identity.GetUserId();

            var user = User as ClaimsPrincipal;
            var topicService = new TopicService(user.FindFirst("access_token").Value);

            var paramList = new Dictionary<string, string>
            {
                { "sort", "id" },
                { "fields","id,title"}
            };

            var model = new SurveyFormViewModel
            {
                Topics = await topicService.GetUserTopicsAsync(Guid.Parse(userId), paramList)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterSurvey(Survey survey)
        {
            var user = User as ClaimsPrincipal;
            var service = new SurveyService(user.FindFirst("access_token").Value);

            var newSurvey = await service.RegisterSurveyAsync(survey);

            if (newSurvey == null)
            {
                return Content("La encuesta no pudo ser registrada");
            }

            return Json(Url.Action("Index", "Topic"));
        }
    }
}
