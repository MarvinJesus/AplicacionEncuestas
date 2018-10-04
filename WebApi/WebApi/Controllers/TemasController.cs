using CoreApi;
using Entities_POJO;
using Exceptions;
using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class TemasController : ApiController
    {
        private ITemaManager _manager { get; set; }
        public IUsuarioManager _usuarioManager { get; set; }

        public TemasController(ITemaManager manager, IUsuarioManager usuarioManager)
        {
            _manager = manager;
            _usuarioManager = usuarioManager;
        }

        [HttpPost]
        [Route("temas")]
        public IHttpActionResult PostTopic([FromBody] Tema tema)
        {
            try
            {
                if (tema == null)
                    return BadRequest();

                var result = _manager.RegistrarTema(tema);

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
            catch (Exception ex)
            {
                ExceptionManager.GetInstance().Process(ex);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("temas/{id}")]
        public IHttpActionResult PutTopic(int id, [FromBody]Tema topic)
        {
            try
            {
                if (topic == null)
                    return BadRequest();

                var result = _manager.ActualizarTema(topic);

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
    }
}