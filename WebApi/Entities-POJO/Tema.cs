using System.Collections.Generic;

namespace Entities_POJO
{
    public class Tema : BaseEntity
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string ImagePath { get; set; }

        public int UsuarioId { get; set; }

        public ICollection<Pregunta> Preguntas { get; set; }

        public Tema()
        {
        }

        public Tema(int id, string titulo, string descripcion, string imagePath, int usuarioId)
        {
            Id = id;
            Titulo = titulo;
            Descripcion = descripcion;
            ImagePath = imagePath;
            UsuarioId = usuarioId;
        }
    }
}