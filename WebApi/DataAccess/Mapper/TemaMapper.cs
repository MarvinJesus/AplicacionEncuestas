using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class TemaMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "Id";
        private const string DB_COL_TITULO = "Titulo";
        private const string DB_COL_DESCRIPCION = "Descripcion";
        private const string DB_COL_FECHADECREACION = "FechaDeCreacion";
        private const string DB_COL_IMAGEPATH = "ImagePath";
        private const string DB_COL_USUARIOID = "UsuarioId";


        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "CREATE_TEMA" };

            var tema = (Tema)entity;

            operation.AddVarcharParam(DB_COL_TITULO, tema.Titulo);
            operation.AddVarcharParam(DB_COL_DESCRIPCION, tema.Descripcion);
            operation.AddDateParam(DB_COL_FECHADECREACION, tema.FechaDeCreacion);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, tema.ImagePath);
            operation.AddIntParam(DB_COL_USUARIOID, tema.UsuarioId);

            return operation;
        }



        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_TEMA" };
            var tema = (Tema)entity;
            operation.AddIntParam(DB_COL_ID, tema.Id);
            return operation;
        }



        public SqlOperation GetRetriveAllStatement()
        {
            var operation = new SqlOperation { ProcedureName = "RET_All_TEMAS" };

            return operation;
        }

        public SqlOperation GetRetriveTemasByUser(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_TEMAS_BY_USER" };
            var tema = (Tema)entity;
            operation.AddIntParam(DB_COL_USUARIOID, tema.UsuarioId);
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "UPD_TEMA" };
            var tema = (Tema)entity;

            operation.AddIntParam(DB_COL_ID, tema.Id);
            operation.AddVarcharParam(DB_COL_TITULO, tema.Titulo);
            operation.AddVarcharParam(DB_COL_DESCRIPCION, tema.Descripcion);
            operation.AddDateParam(DB_COL_FECHADECREACION, tema.FechaDeCreacion);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, tema.ImagePath);
            operation.AddIntParam(DB_COL_USUARIOID, tema.UsuarioId);

            return operation;
        }



        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "DEL_TEMA" };
            var tema = (Tema)entity;
            operation.AddIntParam(DB_COL_ID, tema.Id);
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
            var Tema = new Tema
            {
                Id = GetIntValue(row, DB_COL_ID),
                Titulo = GetStringValue(row, DB_COL_TITULO),
                Descripcion = GetStringValue(row, DB_COL_DESCRIPCION),
                FechaDeCreacion = GetDateValue(row, DB_COL_FECHADECREACION),
                ImagePath = GetStringValue(row, DB_COL_IMAGEPATH),
                UsuarioId = GetIntValue(row, DB_COL_USUARIOID)

            };
            return Tema;
        }
    }
}
