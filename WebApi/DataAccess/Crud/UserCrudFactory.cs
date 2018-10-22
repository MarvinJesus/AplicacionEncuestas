using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class UserCrudFactory : CrudFactory
    {
        private SqlDao SqlDao { get; set; }
        private UserMapper _mapper { get; set; }

        public UserCrudFactory()
        {
            SqlDao = SqlDao.GetInstance();
            _mapper = new UserMapper();
        }
        public override T Create<T>(BaseEntity entity)
        {
            var operation = _mapper.GetCreateStatement(entity);
            var lstResult = SqlDao.ExecuteQueryProcedure(operation);

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
            throw new NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override ICollection<T> RetrieveAll<T>()
        {
            throw new NotImplementedException();
        }

        public override int Update(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
