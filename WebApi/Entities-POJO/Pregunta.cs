using System.Collections.Generic;

namespace Entities_POJO
{
    public class Pregunta : BaseEntity
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int IdTema { get; set; }

        public ICollection<Respuesta> Repuestas { get; set; }

        public Pregunta()
        {

        }

        public Pregunta(int id, string descripcion, int idTema)
        {
            Id = id;
            Descripcion = descripcion;
            IdTema = idTema;
        }
    }
}
