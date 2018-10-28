using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class QuestionCrudFactory : CrudFactory
    {
        private QuestionMapper _mapper { get; set; }

        public QuestionCrudFactory()
        {
            _mapper = new QuestionMapper();
            dao = SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var question = (Question)entity;
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

        public ICollection<T> GetAllQuestionsBySurvey<T>(BaseEntity entity)
        {
            var lstQuestion = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveQuestionsBySurvey(entity));
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
            var question = (Question)entity;
            return dao.ExecuteProcedure(_mapper.GetUpdateStatement(question));
        }

        public override int Delete(BaseEntity entity)
        {
            var question = (Question)entity;
            return dao.ExecuteProcedure(_mapper.GetDeleteStatement(question));
        }
    }
}
