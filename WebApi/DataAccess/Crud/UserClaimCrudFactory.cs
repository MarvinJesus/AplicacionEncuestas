using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class UserClaimCrudFactory : CrudFactory
    {
        private UserClaimMapper _mapper { get; set; }

        public UserClaimCrudFactory()
        {
            dao = SqlDao.GetInstance();
            _mapper = new UserClaimMapper();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var userClaim = (UserClaim)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(userClaim));

            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                dic = lstResult[0];
                var objs = _mapper.BuildObject(dic);
                return (T)Convert.ChangeType(objs, typeof(T));
            }

            return default(T);
        }

        public ICollection<UserClaim> RetrieveUserClaimByUser(BaseEntity entity)
        {
            var lstUserClaims = new List<UserClaim>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetUserClaims(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstUserClaims.Add((UserClaim)c);
                }
            }

            return lstUserClaims;
        }

        public override int Delete(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override ICollection<T> RetrieveAll<T>()
        {
            throw new System.NotImplementedException();
        }

        public override int Update(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
