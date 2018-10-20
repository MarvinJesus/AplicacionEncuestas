using DataAccess.Dao;
using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class TopicCrudFactory : CrudFactory
    {
        private TopicMapper _mapper { get; set; }

        public TopicCrudFactory()
        {
            _mapper = new TopicMapper();
            dao = SqlDao.GetInstance();
        }

        public override T Create<T>(BaseEntity entity)
        {
            var Topic = (Topic)entity;
            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetCreateStatement(Topic));

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
            var Topic = (Topic)entity;
            return dao.ExecuteProcedure(_mapper.GetDeleteStatement(Topic));
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
            var lstTopics = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveAllStatement());
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstTopics.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstTopics;
        }

        public override int Update(BaseEntity entity)
        {
            var topic = (Topic)entity;
            return dao.ExecuteProcedure(_mapper.GetUpdateStatement(topic));
        }

        public ICollection<T> GetAllTopicsByUser<T>(BaseEntity entity)
        {
            var lstTopics = new List<T>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetriveTopicsByUser(entity));
            var dic = new Dictionary<string, object>();
            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstTopics.Add((T)Convert.ChangeType(c, typeof(T)));
                }
            }

            return lstTopics;
        }
    }
}
