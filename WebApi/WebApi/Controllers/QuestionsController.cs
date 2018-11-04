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
    public class QuestionsController : ApiController
    {
        private IQuestionManager _manager { get; set; }
        private IAnswerManager _answerManager { get; set; }
        private QuestionFactory QuestionFactory { get; set; }
        private const string ANSWER_PROPERTY = "answers";

        public QuestionsController(IQuestionManager QuestionManager, IAnswerManager answerManager)
        {
            _manager = QuestionManager;
            _answerManager = answerManager;
            QuestionFactory = new QuestionFactory();
        }

        [HttpPost]
        [ScopeAuthorize("write")]
        [Route("questions")]
        public IHttpActionResult PostQuestion([FromBody]Question question)
        {
            try
            {
                if (question == null)
                    return BadRequest();

                var result = _manager.RegisterQuestion(question);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                {
                    if (question.Answers != null)
                    {
                        var answersResult = _answerManager.RegisterAnwers(result.Entity.Id, question.Answers);

                        if (answersResult.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                        {
                            result.Entity.Answers = answersResult.Entity;
                        }
                        else
                        {
                            if (answersResult.Status == CoreApi.ActionResult.ManagerActionStatus.Error && answersResult.Exception != null)
                            {
                                return BadRequest(answersResult.Exception.AppMessage.Message);
                            }

                            return BadRequest();
                        }
                    }

                    return Created(Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("surveys/{id}/questions")]
        public IHttpActionResult GetQuestionsbySurvey(Guid id, string sort = "id", string fields = null)
        {
            try
            {
                var questions = _manager.GetQuestionsBySurvey(id);

                if (questions == null) return NotFound();

                var questionsList = questions.AsQueryable().ApplySort(sort);

                if (fields != null)
                {
                    var listOfFields = fields.ToLower().Split(',').ToList();

                    if (fields.Contains(ANSWER_PROPERTY))
                    {
                        foreach (var question in questionsList)
                        {
                            question.Answers = _answerManager.GetAnswersByQuestion(question.Id);
                        }
                    }

                    return Ok(questionsList.Select(q => QuestionFactory.CreateDataShapeObject(q, listOfFields)));
                }

                return Ok(questionsList.Select(q => QuestionFactory.CreateDataShapeObject(q)));
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("surveys/{surveyId}/questions/{id}")]
        [Route("questions/{id}")]
        public IHttpActionResult GetQuestion(int id, Guid? surveyId = null, string fields = null)
        {
            try
            {
                Question question = null;

                if (surveyId == null)
                {
                    question = _manager.GetQuestion(id);
                }
                else
                {
                    ICollection<Question> questions = _manager.GetQuestionsBySurvey((Guid)surveyId);

                    if (questions?.Count > 0)
                    {
                        question = questions.FirstOrDefault(a => a.Id == id);
                    }
                }

                if (question == null) return NotFound();

                if (fields != null)
                {
                    var listOfFields = fields.ToLower().Split(',').ToList();

                    if (fields.Contains(ANSWER_PROPERTY))
                    {
                        question.Answers = _answerManager.GetAnswersByQuestion(question.Id);
                    }

                    return Ok(QuestionFactory.CreateDataShapeObject(question, listOfFields));
                }

                return Ok(QuestionFactory.CreateDataShapeObject(question));
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        [ScopeAuthorize("write")]
        [Route("questions/{id}")]
        public IHttpActionResult PutQuestion(int id, [FromBody]Question question)
        {
            try
            {
                if (question == null)
                    return BadRequest();

                var result = _manager.UpdateQuestion(question);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                {
                    if (question.Answers != null)
                    {
                        var answersResult = _answerManager.UpdateAnswers(question.Id, question.Answers);

                        if (answersResult.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                        {
                            result.Entity.Answers = answersResult.Entity;
                        }
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
        [Route("surveys/{surveyId}/questions/{id}")]
        public IHttpActionResult DeleteQuestion(int id, Guid surveyId)
        {
            try
            {
                var result = _manager.DeleteQuestion(id, surveyId);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Deleted)
                    return StatusCode(System.Net.HttpStatusCode.NoContent);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }
    }
}
