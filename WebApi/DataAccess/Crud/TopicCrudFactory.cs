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

        public ICollection<Category> RetrieveCategoryByTopic(BaseEntity entity)
        {
            var lstCategories = new List<Category>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetRetrieveCategoryByTopic(entity));

            if (lstResult.Count > 0)
            {
                var objs = new CategoryMapper().BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstCategories.Add((Category)c);
                }
            }

            return lstCategories;
        }

        public Category CreateCategoryByTopic(BaseEntity topic, BaseEntity category)
        {
            var result = dao.ExecuteQueryProcedure(_mapper.GetCreateCategoryByTopic(topic, category));

            var dic = new Dictionary<string, object>();
            if (result.Count > 0)
            {
                dic = result[0];
                var objs = new CategoryMapper().BuildObject(dic);
                return (Category)objs;
            }

            return default(Category);
        }

        public int DeleteTopicsCategory(BaseEntity entity)
        {
            var topic = (Topic)entity;
            return dao.ExecuteProcedure(_mapper.GetDeleteTopicsCategory(topic));
        }

        public ICollection<Topic> SearchTopic(string search)
        {
            var lstTopics = new List<Topic>();

            var lstResult = dao.ExecuteQueryProcedure(_mapper.GetSearchTopics(search));

            if (lstResult.Count > 0)
            {
                var objs = _mapper.BuildObjects(lstResult);
                foreach (var c in objs)
                {
                    lstTopics.Add((Topic)c);
                }
            }

            return lstTopics;
        }
    }
}
