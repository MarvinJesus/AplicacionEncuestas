using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class PreguntasController : ApiController
    {
        public IPreguntaManager _manager { get; set; }

        public PreguntasController(IPreguntaManager preguntaManager)
        {
            _manager = preguntaManager;
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
        [Route("preguntas")]
        public IHttpActionResult PostQuestion([FromBody]Pregunta question)
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
    }
}
