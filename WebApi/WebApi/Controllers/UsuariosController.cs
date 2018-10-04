using CoreApi;
using DataAccess.Factories;
using Dtos;
using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UsuariosController : ApiController
    {
        private IUsuarioManager _manager { get; set; }

        public UsuariosController(IUsuarioManager usuarioManager)
        {
            _manager = usuarioManager;
        }

        [HttpPost]
        public IHttpActionResult PostUsuario([FromBody]UsuarioDto usuarioDto)
        {
            try
            {
                if (usuarioDto == null)
                    return BadRequest();

                var usuario = UsuarioFactory.CreateUsuario(usuarioDto);

                var newUsuario = _manager.RegistrarUsuario(usuario);

                if (newUsuario != null)
                {
                    return Created<UsuarioDto>
                        (Request.RequestUri + "/" + newUsuario.Id.ToString(), UsuarioFactory.CreateUsuario(newUsuario));
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        // PUT: api/Usuarios/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Usuarios/5
        public void Delete(int id)
        {
        }
    }
}
