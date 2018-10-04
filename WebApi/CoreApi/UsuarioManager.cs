using CoreApi.ActionResult;
using DataAccess.Crud;
using Dtos;
using Entities_POJO;
using Exceptions;

namespace CoreApi
{
    public class UsuarioManager : IUsuarioManager
    {
        private UsuarioCrudFactory _crudFactory { get; set; }

        public UsuarioManager()
        {
            _crudFactory = new UsuarioCrudFactory();
        }

        public Usuario GetUsuario(Usuario usuario)
        {
            return _crudFactory.Retrieve<Usuario>(usuario);
        }

        public ManagerActionResult<Usuario> RegistrarUsuario(UsuarioDto usuarioDto)
        {
            try
            {
                if (usuarioDto.Contrasenia == null)
                    throw new BussinessException(2);

                Usuario usuario = DataAccess.Factories.UsuarioFactory.CreateUsuario(usuarioDto);

                var newUser = _crudFactory.Create<Usuario>(usuario);

                if (newUser != null)
                {
                    return new ManagerActionResult<Usuario>(newUser, ManagerActionStatus.Created);
                }
                else
                {
                    return new ManagerActionResult<Usuario>(newUser, ManagerActionStatus.NothingModified, null);
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                BussinessException exception;

                switch (sqlEx.Number)
                {
                    case 201:
                        //Missing parameters
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(2));
                        break;
                    case 2627:
                        //Existing user
                        exception = ExceptionManager.GetInstance().Process(new BussinessException(3));
                        break;
                    default:
                        //Uncontrolled exception
                        exception = ExceptionManager.GetInstance().Process(sqlEx);
                        break;
                }

                return new ManagerActionResult<Usuario>(
                    null, ManagerActionStatus.Error, exception);
            }
            catch (System.Exception ex)
            {
                var exception = ExceptionManager.GetInstance().Process(ex);

                return new ManagerActionResult<Usuario>(null, ManagerActionStatus.Error, exception);
            }
        }
    }

    public interface IUsuarioManager
    {
        Usuario GetUsuario(Usuario usuario);
        ManagerActionResult<Usuario> RegistrarUsuario(UsuarioDto usuario);
    }
}
