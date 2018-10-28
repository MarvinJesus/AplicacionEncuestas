using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class QuestionMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "QUESTION_ID";
        private const string DB_COL_DESCRIPTION = "QUESTION_DESCRIPTION";
        private const string DB_COL_SURVEY_ID = "SURVEY_ID";


        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "CRE_QUESTION" };

            var question = (Question)entity;

            operation.AddVarcharParam(DB_COL_DESCRIPTION, question.Description);
            operation.AddGuidParam(DB_COL_SURVEY_ID, question.SurveyId);

            return operation;
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_QUESTION" };
            var question = (Question)entity;
            operation.AddIntParam(DB_COL_ID, question.Id);
            return operation;
        }

        public SqlOperation GetRetriveQuestionsBySurvey(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_QUESTIONS_BY_TOPIC_ID" };
            var question = (Question)entity;
            operation.AddGuidParam(DB_COL_SURVEY_ID, question.SurveyId);
            return operation;
        }

        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation
            {
                ProcedureName = "RET_ALL_QUESTIONS"
            };
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "UPD_QUESTION" };

            var question = (Question)entity;

            operation.AddIntParam(DB_COL_ID, question.Id);
            operation.AddVarcharParam(DB_COL_DESCRIPTION, question.Description);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "DEL_QUESTION" };
            var question = (Question)entity;
            operation.AddIntParam(DB_COL_ID, question.Id);
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
            var Question = new Question
            {
                Id = GetIntValue(row, DB_COL_ID),
                Description = GetStringValue(row, DB_COL_DESCRIPTION),
                SurveyId = GetGuidValue(row, DB_COL_SURVEY_ID)
            };
            return Question;
        }
    }
}
