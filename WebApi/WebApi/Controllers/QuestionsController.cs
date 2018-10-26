using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

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
        [Route("topics/{id}/questions")]
        public IHttpActionResult GetQuestionsbyTopic(Guid id)
        {
            try
            {
                var questions = _manager.GetQuestionsByTopic(id);

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
        [Route("topics/{topicId}/questions/{id}")]
        [Route("questions/{id}")]
        public IHttpActionResult GetQuestion(int id, Guid? topicId = null)
        {
            try
            {
                Question question = null;

                if (topicId == null)
                {
                    question = _manager.GetQuestion(id);
                }
                else
                {
                    ICollection<Question> questions = _manager.GetQuestionsByTopic((Guid)topicId);

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
        [Route("topics/{topicId}/questions/{id}")]
        public IHttpActionResult DeleteAnswer(int id, Guid topicId)
        {
            try
            {
                var result = _manager.DeleteQuestion(id, topicId);

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
