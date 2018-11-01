using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class SurveyMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "SURVEY_ID";
        private const string DB_COL_TITLE = "SURVEY_TITLE";
        private const string DB_COL_DESCRIPTION = "SURVEY_DESCRIPTION";
        private const string DB_COL_IMAGEPATH = "IMG_URL";
        private const string DB_COL_TOPICID = "TOPIC_ID";


        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "CRE_SURVEY" };

            var survey = (Survey)entity;

            operation.AddVarcharParam(DB_COL_TITLE, survey.Title);
            operation.AddVarcharParam(DB_COL_DESCRIPTION, survey.Description);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, survey.ImagePath);
            operation.AddGuidParam(DB_COL_TOPICID, survey.TopicId);

            return operation;
        }



        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_SURVEY" };
            var survey = (Survey)entity;
            operation.AddGuidParam(DB_COL_ID, survey.Id);
            return operation;
        }



        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "RET_ALL_SURVEYS" };

            return operation;
        }

        public SqlOperation GetRetriveSurveysByTopic(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_SURVEY_BY_TOPIC_ID" };
            var survey = (Survey)entity;
            operation.AddGuidParam(DB_COL_TOPICID, survey.TopicId);
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "UPD_SURVEY" };
            var survey = (Survey)entity;

            operation.AddGuidParam(DB_COL_ID, survey.Id);
            operation.AddVarcharParam(DB_COL_TITLE, survey.Title);
            operation.AddVarcharParam(DB_COL_DESCRIPTION, survey.Description);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, survey.ImagePath);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "DEL_SURVEY" };
            var survey = (Survey)entity;
            operation.AddGuidParam(DB_COL_ID, survey.Id);
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
            var survey = new Survey
            {
                Id = GetGuidValue(row, DB_COL_ID),
                Title = GetStringValue(row, DB_COL_TITLE),
                Description = GetStringValue(row, DB_COL_DESCRIPTION),
                ImagePath = GetStringValue(row, DB_COL_IMAGEPATH),
                TopicId = GetGuidValue(row, DB_COL_TOPICID)

            };
            return survey;
        }
    }
}
