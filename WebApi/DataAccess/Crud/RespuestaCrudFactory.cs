using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class RespuestaCrudFactory : CrudFactory
    {
        private RespuestaMapper _mapper { get; set; }

        public RespuestaCrudFactory()
        {
            _mapper = new RespuestaMapper();
            dao = Dao.SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var answer = (Respuesta)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(answer));

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
            var answer = (Respuesta)entity;
            return dao.ExecuteProcedure(_mapper.GetDeleteStatement(answer));
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
            var lstAnswer = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstAnswer.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstAnswer;
        }

        public override int Update(BaseEntity entity)
        {
            var answer = (Respuesta)entity;
            return dao.ExecuteProcedure(_mapper.GetUpdateStatement(answer));
        }

        public ICollection<T> GetAllTemasByUser<T>(BaseEntity entity)
        {
            var lstAnswer = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveAnswersByQuestion(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstAnswer.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstAnswer;
        }
    }
}
