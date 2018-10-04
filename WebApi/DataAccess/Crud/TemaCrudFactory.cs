using DataAccess.Crud;
using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace CoreApi
{
    public class TemaCrudFactory : CrudFactory
    {
        private TemaMapper TemaMapper { get; set; }

        public TemaCrudFactory()
        {
            TemaMapper = new TemaMapper();
            dao = SqlDao.GetInstance();
        }

        public override void Create(BaseEntity entity)
        {
            var tema = (Tema)entity;
            var sqlOperation = TemaMapper.GetCreateStatement(tema);
            dao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseEntity entity)
        {
            var tema = (Tema)entity;
            dao.ExecuteProcedure(TemaMapper.GetDeleteStatement(tema));
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            var lstResult = dao.ExecuteQueryProcedure(TemaMapper.GetRetriveStatement(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                dic = lstResult[0];
                var objs = TemaMapper.BuildObject(dic);
                return (T)Convert.ChangeType(objs, typeof(T));
            }

            return default(T);
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstTemas = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(TemaMapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = TemaMapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstTemas.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstTemas;
        }

        public override void Update(BaseEntity entity)
        {
            var usuario = (Tema)entity;
            dao.ExecuteProcedure(TemaMapper.GetUpdateStatement(usuario));
        }

        public List<T> GetAllTemasByUser<T>(BaseEntity entity)
        {
            var lstTemas = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(TemaMapper.GetRetriveTemasByUser(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = TemaMapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstTemas.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstTemas;
        }
    }
}
