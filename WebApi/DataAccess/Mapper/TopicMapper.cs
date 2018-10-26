using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class TopicMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "TOPIC_ID";
        private const string DB_COL_TITLE = "TITLE";
        private const string DB_COL_DESCRIPTION = "TOPIC_DESCRIPTION";
        private const string DB_COL_IMAGEPATH = "IMG_URL";
        private const string DB_COL_USERID = "USER_ID";


        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "CRE_TOPIC" };

            var Topic = (Topic)entity;

            operation.AddVarcharParam(DB_COL_TITLE, Topic.Title);
            operation.AddVarcharParam(DB_COL_DESCRIPTION, Topic.Description);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, Topic.ImagePath);
            operation.AddGuidParam(DB_COL_USERID, Topic.UserId);

            return operation;
        }



        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_TOPIC" };
            var Topic = (Topic)entity;
            operation.AddGuidParam(DB_COL_ID, Topic.Id);
            return operation;
        }



        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "RET_ALL_TOPICS" };

            return operation;
        }

        public SqlOperation GetRetriveTopicsByUser(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_TOPIC_BY_USER_ID" };
            var Topic = (Topic)entity;
            operation.AddGuidParam(DB_COL_USERID, Topic.UserId);
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "UPD_TOPIC" };
            var Topic = (Topic)entity;

            operation.AddGuidParam(DB_COL_ID, Topic.Id);
            operation.AddVarcharParam(DB_COL_TITLE, Topic.Title);
            operation.AddVarcharParam(DB_COL_DESCRIPTION, Topic.Description);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, Topic.ImagePath);

            return operation;
        }



        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "DEL_TOPIC" };
            var Topic = (Topic)entity;
            operation.AddGuidParam(DB_COL_ID, Topic.Id);
            return operation;
        }



        public List<BaseEntity> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResults = new List<BaseEntity>();
            foreach (var row in lstRows)
            {
                var entity = BuildObject(row);
                lstResults.Add(entity);
            }
            return lstResults;
        }

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var Topic = new Topic
            {
                Id = GetGuidValue(row, DB_COL_ID),
                Title = GetStringValue(row, DB_COL_TITLE),
                Description = GetStringValue(row, DB_COL_DESCRIPTION),
                ImagePath = GetStringValue(row, DB_COL_IMAGEPATH),
                UserId = GetGuidValue(row, DB_COL_USERID)

            };
            return Topic;
        }
    }
}
