using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class PreguntaCrudFactory : CrudFactory
    {
        private PreguntaMapper _mapper { get; set; }

        public PreguntaCrudFactory()
        {
            _mapper = new PreguntaMapper();
            dao = SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var question = (Pregunta)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(question));

            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                dic = lstResult[0];
                var objs = _mapper.BuildObject(dic);
                return (T)Convert.ChangeType(objs, typeof(T));
            }

            return default(T);
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

        public ICollection<T> GetAllQuestionsByTopic<T>(BaseEntity entity)
        {
            var lstQuestion = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveQuestionsByTopic(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstQuestion.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstQuestion;
        }

        public override ICollection<T> RetrieveAll<T>()
        {
            var questionList = new List<T>();
            var questionsListResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (questionsListResult.Count > 0)
            {
                var objects = _mapper.BuildObjects(questionsListResult);
                foreach (var c in objects)
                {
                    questionList.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return questionList;
        }

        public override int Update(BaseEntity entity)
        {
            var question = (Pregunta)entity;
            return dao.ExecuteProcedure(_mapper.GetUpdateStatement(question));
        }

        public override int Delete(BaseEntity entity)
        {
            var question = (Pregunta)entity;
            return dao.ExecuteProcedure(_mapper.GetDeleteStatement(question));
        }
    }
}
