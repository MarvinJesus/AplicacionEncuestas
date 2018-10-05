using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class TemaCrudFactory : CrudFactory
    {
        private TemaMapper _mapper { get; set; }

        public TemaCrudFactory()
        {
            _mapper = new TemaMapper();
            dao = SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var tema = (Tema)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(tema));

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
            var tema = (Tema)entity;
            return dao.ExecuteProcedure(_mapper.GetDeleteStatement(tema));
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

        public override ICollection<T> RetrieveAll<T>()
        {
            var lstTemas = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstTemas.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstTemas;
        }

        public override int Update(BaseEntity entity)
        {
            var topic = (Tema)entity;
            return dao.ExecuteProcedure(_mapper.GetUpdateStatement(topic));
        }

        public ICollection<T> GetAllTemasByUser<T>(BaseEntity entity)
        {
            var lstTemas = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveTemasByUser(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstTemas.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstTemas;
        }
    }
}
