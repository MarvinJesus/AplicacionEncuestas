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
        }

        public override T Create<T>(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(BaseEntity entity)
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

        public override void Update(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
