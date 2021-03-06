﻿using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class AnswerMapper : EntityMapper, IObjectMapper, ISqlStaments
    {
        private const string DB_COL_ID = "ANSWER_ID";
        private const string DB_COL_DESCRIPTION = "ANSWER_DESCRIPTION";
        private const string DB_COL_QUESTIONID = "QUESTION_ID";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var answer = new Answer
            {
                Id = GetIntValue(row, DB_COL_ID),
                Description = GetStringValue(row, DB_COL_DESCRIPTION),
                QuestionId = GetIntValue(row, DB_COL_QUESTIONID)
            };

            return answer;
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

        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "CRE_ANSWER" };

            var answer = (Answer)entity;

            operation.AddVarcharParam(DB_COL_DESCRIPTION, answer.Description);
            operation.AddIntParam(DB_COL_QUESTIONID, answer.QuestionId);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "DEL_ANSWER" };
            var answer = (Answer)entity;
            operation.AddIntParam(DB_COL_ID, answer.Id);
            return operation;
        }

        public SqlOperation GetDeleteAnswersByQuestion(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "DEL_ANSWER_BY_QUESTION" };
            var answer = (Answer)entity;
            operation.AddIntParam(DB_COL_QUESTIONID, answer.QuestionId);
            return operation;
        }

        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "RET_ALL_ANSWERS" };

            return operation;
        }

        public SqlOperation GetRetriveAnswersByQuestion(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_ANSWERS_BY_QUESTION_ID" };
            var answer = (Answer)entity;
            operation.AddIntParam(DB_COL_QUESTIONID, answer.QuestionId);
            return operation;
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_ANSWER" };
            var answer = (Answer)entity;
            operation.AddIntParam(DB_COL_ID, answer.Id);
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "UPD_ANSWER" };

            var answer = (Answer)entity;

            operation.AddIntParam(DB_COL_ID, answer.Id);
            operation.AddVarcharParam(DB_COL_DESCRIPTION, answer.Description);

            return operation;
        }
    }
}
