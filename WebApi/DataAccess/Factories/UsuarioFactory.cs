using Dtos;
using EncryptPassword;
using Entities_POJO;
using System.Text;

namespace DataAccess.Factories
{
    public class UsuarioFactory
    {

        public static Usuario CreateUsuario(UsuarioDto usuarioDto)
        {
            var usuario = new Usuario
            {
                Id = usuarioDto.Id,
                Nombre = usuarioDto.Nombre,
                Cedula = usuarioDto.Cedula,
                Correo = usuarioDto.Correo,
                Salt = Cryptographic.GenerateSalt(),
                ImagePath = usuarioDto.ImagePath
            };

            usuario.Contrasenia = Cryptographic.HashPasswordWithSalt(Encoding.UTF8.GetBytes(usuarioDto.Contrasenia), usuario.Salt);

            return usuario;
        }

        public static UsuarioDto CreateUsuario(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Cedula = usuario.Cedula,
                Correo = usuario.Correo,
                Contrasenia = "",
                ImagePath = usuario.ImagePath
            };
        }
    }
}
