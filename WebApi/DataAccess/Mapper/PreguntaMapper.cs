using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dao;
using Entities_POJO;

namespace DataAccess.Mapper
{
   public class PreguntaMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string BD_COL_ID = "QUESTION_ID";
        private const string DB_COL_DESCRIPCION = "DESCRIPTION";
        private const string BD_COL_ID_TOPIC = "TOPIC_ID";
       

        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation
            {
                ProcedureName= "RET_ALL_QUESTIONS"
            };
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
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
            var Pregunta = new Pregunta
            {
                Id= GetIntValue(row, BD_COL_ID),
                Descripcion = GetStringValue(row,DB_COL_DESCRIPCION),
                IdTema = GetIntValue(row, BD_COL_ID_TOPIC)
            };
            return Pregunta;
        }
    }
}
