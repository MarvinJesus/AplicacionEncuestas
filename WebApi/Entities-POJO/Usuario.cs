namespace Entities_POJO
{
    public class Usuario : BaseEntity
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Cedula { get; set; }

        public string Correo { get; set; }

        public string ImagePath { get; set; }

        public string Contrasenia { get; set; }

        public Usuario()
        {

        }

        public Usuario(int id, string nombre, string cedula, string correo, string contrasenia, string imagePath)
        {
            Id = id;
            Nombre = nombre;
            Cedula = cedula;
            Correo = Correo;
            Contrasenia = contrasenia;
            ImagePath = imagePath;
        }
    }
}
