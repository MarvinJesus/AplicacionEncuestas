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
                    ICollection<Respuesta> answers = _manager.GetAnswersByQuestionId((int)answerId);

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
                var answers = _manager.GetAnswersByQuestionId(id);

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
    }
}
