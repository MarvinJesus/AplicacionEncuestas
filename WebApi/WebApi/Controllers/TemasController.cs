using CoreApi;
using Entities_POJO;
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
        [Route("temas/usuario/{id}/temas")]
        public IHttpActionResult PostTemas(int id, [FromBody] Tema tema)
        {
            try
            {
                var usuario = _usuarioManager.GetUsuario(new Usuario { Id = id });

                if (usuario == null)
                    NotFound();

                if (tema == null)
                    return BadRequest();

                var newTema = _manager.RegistrarTema(tema);

                if (newTema != null)
                {
                    return Created(Request.RequestUri + "/" + newTema.Id, newTema);
                }

                return BadRequest();

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}