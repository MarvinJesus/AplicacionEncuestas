using CoreApi;
using DataAccess.Factories;
using Dtos;
using Exceptions;
using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class UsuariosController : ApiController
    {
        private IUsuarioManager _manager { get; set; }

        public UsuariosController(IUsuarioManager usuarioManager)
        {
            _manager = usuarioManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult PostUsuario([FromBody]UsuarioDto usuarioDto)
        {
            try
            {
                if (usuarioDto == null)
                    return BadRequest();

                var result = _manager.RegistrarUsuario(usuarioDto);

                if (result.Status == CoreApi.ActionResult.ManagerActionStatus.Created)
                {
                    return Created<UsuarioDto>
                        (Request.RequestUri + "/" + result.Entity.Id.ToString(), UsuarioFactory.CreateUsuario(result.Entity));
                }

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
    }
}
