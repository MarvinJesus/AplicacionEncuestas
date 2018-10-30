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
    [RoutePrefix("api")]
    public class QuestionsController : ApiController
    {
        public IQuestionManager _manager { get; set; }

        public QuestionsController(IQuestionManager QuestionManager)
        {
            _manager = QuestionManager;
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        public IHttpActionResult Get()
        {
            try
            {
                var questions = _manager.GetAllQuestions();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
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
                    return Created(Request.RequestUri + "/" + result.Entity.Id.ToString(), result.Entity);

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
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [ScopeAuthorize("read")]
        [Route("surveys/{id}/questions")]
        public IHttpActionResult GetQuestionsbyTopic(Guid id)
        {
            try
            {
                var questions = _manager.GetQuestionsBySurvey(id);

                if (questions?.Count > 0)
                    return Ok(questions);

                return NotFound();
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
        public IHttpActionResult GetQuestion(int id, Guid? surveyId = null)
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

                if (question != null)
                    return Ok(question);

                return NotFound();
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
        [Route("surveys/{surveyId}/questions/{id}")]
        public IHttpActionResult DeleteAnswer(int id, Guid surveyId)
        {
            try
            {
                var result = _manager.DeleteQuestion(id, surveyId);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Deleted)
                    return StatusCode(System.Net.HttpStatusCode.NoContent);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error && result.Exception != null)
                    return InternalServerError();

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
