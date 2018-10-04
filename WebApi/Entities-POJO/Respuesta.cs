namespace Entities_POJO
{
    public class Respuesta : BaseEntity
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int IdPregunta { get; set; }

        public Respuesta()
        {

        }

        public Respuesta(int id, string descripcion, int idPregunta)
        {
            Id = id;
            Descripcion = descripcion;
            IdPregunta = idPregunta;
        }
    }
}
