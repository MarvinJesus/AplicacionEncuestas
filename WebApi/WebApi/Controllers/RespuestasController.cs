using CoreApi;
using Entities_POJO;
using Exceptions;
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
    }
}
