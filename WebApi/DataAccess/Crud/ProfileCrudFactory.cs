using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class ProfileCrudFactory : CrudFactory
    {
        private ProfileMapper _mapper { get; set; }

        public ProfileCrudFactory()
        {
            _mapper = new ProfileMapper();
            dao = Dao.SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var Profile = (Profile)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(Profile));

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

        public ICollection<Role> RetrieveUserRoles(BaseEntity entity)
        {
            var lstRoles = new List<Role>();
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetrieveUserRoles(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var roleMapper = new RoleMapper();
                var objs = roleMapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstRoles.Add((Role)c);
                }
            }

            return lstRoles;
        }

        public int EditPicture(BaseEntity entity)
        {
            var profile = (Profile)entity;
            return dao.ExecuteProcedure(_mapper.GetEditPicture(profile));
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
