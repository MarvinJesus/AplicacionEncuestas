using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace WebApi.Controllers
{
    public class SurveysController : SurveyOnlineController
    {
        private ISurveyManager _manager { get; set; }
        private IQuestionManager _questionManager { get; set; }

        public SurveysController(ISurveyManager manager, IQuestionManager questionManager)
        {
            _manager = manager;
            _questionManager = questionManager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("topics/{topicId}/surveys/{surveyId}")]
        [Route("surveys/{surveyId}")]
        public IHttpActionResult GetSurvey(Guid surveyId, Guid? topicId = null)
        {
            try
            {
                Survey Survey = new Survey();

                if (topicId == null)
                {
                    Survey = _manager.GetSurvey(surveyId);
                }
                else
                {
                    ICollection<Survey> surveys = _manager.GetSurveysByTopic((Guid)topicId);

                    if (surveys != null)
                        Survey = surveys.FirstOrDefault(t => t.Id == surveyId);
                }

                if (Survey != null)
                    return Ok(Survey);

                return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("topics/{topicId}/surveys")]
        public IHttpActionResult GetSurveysByTopic(Guid topicId)
        {
            try
            {
                var surveys = _manager.GetSurveysByTopic(topicId);

                if (surveys.Count > 0)
                    return Ok(surveys);

                return NotFound();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [ScopeAuthorize("write")]
        [Route("topics/{topicId}/surveys")]
        public IHttpActionResult PostSurvey([FromBody] Survey survey, Guid topicId)
        {
            try
            {
                if (survey == null) return BadRequest();

                var result = _manager.RegisterSurvey(topicId, survey);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                {
                    if (survey.Questions != null)
                    {
                        var questionResult = _questionManager.RegisterQuestions(result.Entity.Id, survey.Questions);

                        if (questionResult.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                        {
                            result.Entity.Questions = questionResult.Entity;

                            return Created(Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);
                        }
                        else
                        {
                            if (questionResult.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                            {
                                return BadRequest(questionResult.Exception.AppMessage.Message);
                            }
                        }
                    }
                    else
                    {
                        return Created(Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);
                    }
                }
                else
                {
                    if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
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

        [HttpPut]
        [ScopeAuthorize("write")]
        [Route("topics/{topicId}/surveys/{id}")]
        public IHttpActionResult PutSurvey(Guid topicId, Guid id, [FromBody]Survey survey)
        {
            try
            {
                if (survey == null)
                    return BadRequest();

                var result = _manager.EditSurvey(survey);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                    return Ok(result.Entity);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                    return InternalServerError();

                return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [ScopeAuthorize("write")]
        [Route("topics/{topicId}/surveys/{id}")]
        public IHttpActionResult DeleteSurvey(Guid id, Guid topicId)
        {
            try
            {
                var result = _manager.DeleteSurvey(id, topicId);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Deleted)
                    return StatusCode(System.Net.HttpStatusCode.NoContent);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error && result.Exception != null)
                    return InternalServerError();

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
