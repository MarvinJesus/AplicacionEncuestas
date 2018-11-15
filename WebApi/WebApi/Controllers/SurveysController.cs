using CoreApi;
using DataAccess.Factory;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;
using WebApi.Helper;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class SurveysController : SurveyOnlineController
    {
        private ISurveyManager _manager { get; set; }
        private IQuestionManager _questionManager { get; set; }
        private IAnswerManager _answerManager { get; set; }
        private SurveyFactory SurveyFactory { get; set; }
        private const string QUESTION_PROPERTY = "questions";
        private const string ANSWER_PROPERTY = "answers";
        private const string IMAGE_PATH_PROPERTY = "imagepath";

        public SurveysController(ISurveyManager manager, IQuestionManager questionManager, IAnswerManager answerManager)
        {
            _manager = manager;
            _answerManager = answerManager;
            _questionManager = questionManager;
            SurveyFactory = new SurveyFactory();
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("topics/{topicId}/surveys/{surveyId}")]
        [Route("surveys/{surveyId}")]
        public IHttpActionResult GetSurvey(Guid surveyId, Guid? topicId = null, string fields = null)
        {
            try
            {
                Survey survey = new Survey();

                if (topicId == null)
                {
                    survey = _manager.GetSurvey(surveyId);
                }
                else
                {
                    ICollection<Survey> surveys = _manager.GetSurveysByTopic((Guid)topicId);

                    if (surveys != null)
                        survey = surveys.FirstOrDefault(t => t.Id == surveyId);
                }

                if (survey != null)
                {
                    survey.ImagePath = string.Concat(PathForPicture.GetInstance().GetPicturePath(Request.RequestUri.PathAndQuery,
                        Request.RequestUri.AbsoluteUri), survey.ImagePath);

                    if (fields != null)
                    {
                        var listOfFields = fields.ToLower().Split(',').ToList();

                        if (fields.Contains(QUESTION_PROPERTY))
                        {
                            survey.Questions = _questionManager.GetQuestionsBySurvey(survey.Id);
                        }

                        if (fields.Contains(ANSWER_PROPERTY))
                        {
                            foreach (var question in survey.Questions)
                            {
                                question.Answers = _answerManager.GetAnswersByQuestion(question.Id);
                            }
                        }

                        return Ok(SurveyFactory.CreateDataShapeObject(survey, listOfFields));
                    }

                    return Ok(SurveyFactory.CreateDataShapeObject(survey));
                }

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
        public IHttpActionResult GetSurveysByTopic(Guid topicId, string fields = null, string sort = "id")
        {
            try
            {
                var surveys = _manager.GetSurveysByTopic(topicId);
                IQueryable<Survey> surveysResult = null;

                if (surveys.Count > 0)
                {
                    if (fields == null || fields.Contains(IMAGE_PATH_PROPERTY))
                    {
                        var path = PathForPicture.GetInstance().GetPicturePath(Request.RequestUri.PathAndQuery,
                            Request.RequestUri.AbsoluteUri);

                        foreach (var survey in surveys)
                        {
                            survey.ImagePath = string.Concat(path, survey.ImagePath);
                        }
                    }

                    surveysResult = surveys.AsQueryable().ApplySort(sort);

                    if (fields != null)
                    {
                        var listOfFields = fields.ToLower().Split(',').ToList();

                        if (fields.Contains(QUESTION_PROPERTY))
                        {
                            foreach (var survey in surveysResult)
                            {
                                survey.Questions = _questionManager.GetQuestionsBySurvey(survey.Id);
                            }
                        }

                        if (fields.Contains(ANSWER_PROPERTY))
                        {
                            foreach (var survey in surveysResult)
                            {
                                foreach (var question in survey.Questions)
                                {
                                    question.Answers = _answerManager.GetAnswersByQuestion(question.Id);
                                }
                            }
                        }

                        return Ok(surveysResult.Select(s => SurveyFactory.CreateDataShapeObject(s, listOfFields)));
                    }

                    return Ok(surveysResult.Select(s => SurveyFactory.CreateDataShapeObject(s)));
                }

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
        public IHttpActionResult PostSurvey([FromBody] SurveyForRegistration surveyForRegistration, Guid topicId)
        {
            try
            {
                if (surveyForRegistration == null) return BadRequest();

                var survey = SurveyFactory.CreateSurvey(surveyForRegistration);

                if (surveyForRegistration.Picture != null)
                {
                    survey.ImagePath = new CreatePictures().CreatePicture(surveyForRegistration.Picture);
                }

                var result = _manager.RegisterSurvey(topicId, survey);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                {
                    if (survey.Questions != null)
                    {
                        var questionResult = _questionManager.RegisterQuestions(result.Entity.Id, survey.Questions);

                        if (questionResult.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                        {
                            result.Entity.Questions = questionResult.Entity;

                            survey.ImagePath = string.Concat(PathForPicture.GetInstance().GetPicturePath(Request.RequestUri.PathAndQuery,
                                    Request.RequestUri.AbsoluteUri), survey.ImagePath);

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
        public IHttpActionResult PutSurvey(Guid topicId, Guid id, [FromBody]SurveyForRegistration surveyForRegistration)
        {
            try
            {
                if (surveyForRegistration == null)
                    return BadRequest();

                var survey = SurveyFactory.CreateSurvey(surveyForRegistration);

                if (surveyForRegistration.Picture != null)
                {
                    survey.ImagePath = new CreatePictures().CreatePicture(surveyForRegistration.Picture);
                }

                var result = _manager.EditSurvey(survey);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                {
                    if (survey.Questions != null)
                    {
                        var questionResult = _questionManager.UpdateQuestions(survey.Questions, survey.Id);

                        if (questionResult.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                        {
                            result.Entity.Questions = questionResult.Entity;
                        }
                    }
                    else
                    {
                        _questionManager.DeleteQuestionBySurvey(survey.Id);
                    }

                    return Ok(result.Entity);
                }

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
