using CoreApi;
using Entities_POJO;
using Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class AnswersController : ApiController
    {
        private IAnswerManager _manager { get; set; }

        public AnswersController(IAnswerManager manger)
        {
            _manager = manger;
        }

        [HttpPost]
        [Route("answers")]
        public IHttpActionResult PostAnswer([FromBody]Answer answer)
        {
            try
            {
                if (answer == null)
                    return BadRequest();

                var result = _manager.RegisterAnswer(answer);

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
        [Route("questions/{answerId}/answers/{id}")]
        [Route("answers/{id}")]
        public IHttpActionResult GetAnswer(int id, int? answerId = null)
        {
            try
            {
                Answer answer = null;

                if (answerId == null)
                {
                    answer = _manager.GetAnswer(id);
                }
                else
                {
                    ICollection<Answer> answers = _manager.GetAnswersByQuestion((int)answerId);

                    if (answers?.Count > 0)
                    {
                        answer = answers.FirstOrDefault(a => a.Id == id);
                    }
                }

                if (answer != null)
                    return Ok(answer);

                return NotFound();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("questions/{id}/answers")]
        public IHttpActionResult GetAnswersbyQuestion(int id)
        {
            try
            {
                var answers = _manager.GetAnswersByQuestion(id);

                if (answers?.Count > 0)
                    return Ok(answers);

                return NotFound();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("answers/{id}")]
        public IHttpActionResult PutAnswer(int id, [FromBody]Answer answer)
        {
            try
            {
                if (answer == null)
                    return BadRequest();

                var result = _manager.UpdateAnswer(answer);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.NotFound)
                    return NotFound();

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Updated)
                    return Ok(result.Entity);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Error)
                    return InternalServerError();

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("questions/{questionId}/answers/{id}")]
        public IHttpActionResult DeleteAnswer(int id, int questionId)
        {
            try
            {
                var result = _manager.DeleteAnswer(id, questionId);

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
