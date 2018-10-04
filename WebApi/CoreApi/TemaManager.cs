using Entities_POJO;

namespace CoreApi
{
    public class TemaManager : ITemaManager
    {
        private TemaCrudFactory _crudFactory { get; set; }

        public TemaManager()
        {
            _crudFactory = new TemaCrudFactory();
        }

        public Tema RegistrarTema(Tema tema)
        {
            return _crudFactory.Create<Tema>(tema);
        }
    }



    public interface ITemaManager
    {
        Tema RegistrarTema(Tema tema);
    }
}
