using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class RespuestaMapper : EntityMapper, IObjectMapper, ISqlStaments
    {
        private const string DB_COL_ID = "ANSWER_ID";
        private const string DB_COL_DESCRIPCION = "ANSWER_DESCRIPTION";
        private const string DB_COL_QUESTIONID = "QUESTION_ID";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var answer = new Respuesta
            {
                Id = GetIntValue(row, DB_COL_ID),
                Descripcion = GetStringValue(row, DB_COL_DESCRIPCION),
                IdPregunta = GetIntValue(row, DB_COL_QUESTIONID)
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

            var answer = (Respuesta)entity;

            operation.AddVarcharParam(DB_COL_DESCRIPCION, answer.Descripcion);
            operation.AddIntParam(DB_COL_QUESTIONID, answer.IdPregunta);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "DEL_ANSWER" };
            var answer = (Respuesta)entity;
            operation.AddIntParam(DB_COL_ID, answer.Id);
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
            var answer = (Respuesta)entity;
            operation.AddIntParam(DB_COL_QUESTIONID, answer.IdPregunta);
            return operation;
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_ANSWER" };
            var answer = (Respuesta)entity;
            operation.AddIntParam(DB_COL_ID, answer.Id);
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "UPD_ANSWER" };

            var answer = (Respuesta)entity;

            operation.AddIntParam(DB_COL_ID, answer.IdPregunta);
            operation.AddVarcharParam(DB_COL_DESCRIPCION, answer.Descripcion);

            return operation;
        }
    }
}
