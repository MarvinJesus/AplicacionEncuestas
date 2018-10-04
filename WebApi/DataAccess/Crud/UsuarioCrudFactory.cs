using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class UsuarioCrudFactory : CrudFactory
    {
        private UsuarioMapper _mapper { get; set; }

        public UsuarioCrudFactory()
        {
            _mapper = new UsuarioMapper();
            dao = Dao.SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var usuario = (Usuario)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(usuario));

            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                dic = lstResult[0];
                var objs = _mapper.BuildObject(dic);
                return (T)Convert.ChangeType(objs, typeof(T));
            }

            return default(T);
        }

        public override int Delete(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveStatement(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                dic = lstResult[0];
                var objs = _mapper.BuildObject(dic);
                return (T)Convert.ChangeType(objs, typeof(T));
            }

            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            throw new System.NotImplementedException();
        }

        public override int Update(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
