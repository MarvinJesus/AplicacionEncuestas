using DataAccess.Crud;
using Entities_POJO;

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

        Usuario IUsuarioManager.RegistrarUsuario(Usuario usuario)
        {
            return _crudFactory.Create<Usuario>(usuario);
        }
    }

    public interface IUsuarioManager
    {
        Usuario GetUsuario(Usuario usuario);
        Usuario RegistrarUsuario(Usuario usuario);
    }
}
