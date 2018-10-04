namespace Entities_POJO
{
    public class Usuario : BaseEntity
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Cedula { get; set; }

        public string Correo { get; set; }

        public string ImagePath { get; set; }

        public byte[] Salt { get; set; }

        public byte[] Contrasenia { get; set; }

        public Usuario()
        {

        }

        public Usuario(int id, string nombre, string cedula, string correo, byte[] contrasenia, string imagePath, byte[] salt)
        {
            Id = id;
            Nombre = nombre;
            Cedula = cedula;
            Correo = Correo;
            Contrasenia = contrasenia;
            ImagePath = imagePath;
            Salt = salt;
        }
    }
}
