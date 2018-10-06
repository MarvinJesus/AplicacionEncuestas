using CoreApi;
using Entities_POJO;
using Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class RespuestasController : ApiController
    {
        private IRespuestaManager _manager { get; set; }

        public RespuestasController(IRespuestaManager manger)
        {
            _manager = manger;
        }

        [HttpPost]
        [Route("respuestas")]
        public IHttpActionResult PostRespuesta([FromBody]Respuesta answer)
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
        [Route("preguntas/{answerId}/respuestas/{id}")]
        [Route("respuestas/{id}")]
        public IHttpActionResult GetRespuesta(int id, int? answerId = null)
        {
            try
            {
                Respuesta answer = null;

                if (answerId == null)
                {
                    answer = _manager.GetAnswer(id);
                }
                else
                {
                    ICollection<Respuesta> answers = _manager.GetAnswersByQuestion((int)answerId);

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
        [Route("preguntas/{id}/respuestas")]
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
        [Route("respuestas/{id}")]
        public IHttpActionResult PutAnswer(int id, [FromBody]Respuesta answer)
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
        [Route("preguntas/{questionId}/respuestas/{id}")]
        public IHttpActionResult DeleteAnswer(int id, int questionId)
        {
            try
            {
                var result = _manager.DeleteAnswer(id, questionId);

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
