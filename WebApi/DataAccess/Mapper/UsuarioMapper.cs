using DataAccess.Dao;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class UsuarioMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "Id";
        private const string DB_COL_CEDULA = "Cedula";
        private const string DB_COL_CORREO = "Correo";
        private const string DB_COL_NOMBRE = "Nombre";
        private const string DB_COL_IMAGEPATH = "ImagePath";
        private const string DB_COL_CONTRASENIA = "Contrasenia";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var usuario = new Usuario
            {
                Id = GetIntValue(row, DB_COL_ID),
                Cedula = GetStringValue(row, DB_COL_CEDULA),
                Correo = GetStringValue(row, DB_COL_CORREO),
                Nombre = GetStringValue(row, DB_COL_NOMBRE),
                ImagePath = GetStringValue(row, DB_COL_IMAGEPATH),
                Contrasenia = GetStringValue(row, DB_COL_CONTRASENIA)
            };
            return usuario;
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

        SqlOperation ISqlStaments.GetCreateStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        SqlOperation ISqlStaments.GetDeleteStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        SqlOperation ISqlStaments.GetRetriveAllStatement()
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_USUARIO" };
            var usuario = (Usuario)entity;
            operation.AddIntParam(DB_COL_ID, usuario.Id);
            return operation;
        }

        SqlOperation ISqlStaments.GetUpdateStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
