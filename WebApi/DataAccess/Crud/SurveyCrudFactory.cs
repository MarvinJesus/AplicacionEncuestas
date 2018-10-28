using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class SurveyCrudFactory : CrudFactory
    {
        private SurveyMapper _mapper { get; set; }

        public SurveyCrudFactory()
        {
            _mapper = new SurveyMapper();
            dao = SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var survey = (Survey)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(survey));

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
            var survey = (Survey)entity;
            return dao.ExecuteProcedure(_mapper.GetDeleteStatement(survey));
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
            var lstSurveys = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstSurveys.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstSurveys;
        }

        public override int Update(BaseEntity entity)
        {
            var survey = (Survey)entity;
            return dao.ExecuteProcedure(_mapper.GetUpdateStatement(survey));
        }

        public ICollection<T> GetAllSurveysByTopic<T>(BaseEntity entity)
        {
            var lstSurveys = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveSurveysByTopic(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstSurveys.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstSurveys;
        }
    }
}
