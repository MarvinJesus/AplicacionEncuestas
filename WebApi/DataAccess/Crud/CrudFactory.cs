using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public abstract class CrudFactory
    {
        protected SqlDao dao;
        public string COMPONENT = "DATA_ACCESS";

        public abstract T Create<T>(BaseEntity entity);
        public abstract T Retrieve<T>(BaseEntity entity);
        public abstract List<T> RetrieveAll<T>();
        public abstract int Update(BaseEntity entity);
        public abstract int Delete(BaseEntity entity);
    }
}
