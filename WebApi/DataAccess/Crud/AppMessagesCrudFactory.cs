using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class AppMessagesCrudFactory : CrudFactory
    {
        AppMessageMapper mapper;
        public AppMessagesCrudFactory()
        {
            mapper = new AppMessageMapper();
            dao = SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstAppMessage = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstAppMessage.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstAppMessage;
        }

        public override int Update(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
