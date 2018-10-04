using DataAccess.Dao;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class UsuarioMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "PROFILE_ID";
        private const string DB_COL_CEDULA = "IDENTIFICATION";
        private const string DB_COL_CORREO = "EMAIL";
        private const string DB_COL_NOMBRE = "NAME";
        private const string DB_COL_IMAGEPATH = "IMG_URL";
        private const string DB_COL_CONTRASENIA = "PASSWORD";
        private const string DB_COL_SALT = "SALT";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var usuario = new Usuario
            {
                Id = GetIntValue(row, DB_COL_ID),
                Cedula = GetStringValue(row, DB_COL_CEDULA),
                Correo = GetStringValue(row, DB_COL_CORREO),
                Nombre = GetStringValue(row, DB_COL_NOMBRE),
                ImagePath = GetStringValue(row, DB_COL_IMAGEPATH),
                Contrasenia = GetBytesValue(row, DB_COL_CONTRASENIA),
                Salt = GetBytesValue(row, DB_COL_SALT)
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

        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var usuario = (Usuario)entity;
            var operation = new SqlOperation { ProcedureName = "CRE_PROFILE" };

            operation.AddVarcharParam(DB_COL_CEDULA, usuario.Cedula);
            operation.AddVarcharParam(DB_COL_CORREO, usuario.Correo);
            operation.AddVarcharParam(DB_COL_NOMBRE, usuario.Nombre);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, usuario.ImagePath);
            operation.AddVarBinaryParam(DB_COL_CONTRASENIA, usuario.Contrasenia);
            operation.AddVarBinaryParam(DB_COL_SALT, usuario.Salt);

            return operation;
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
            var operation = new SqlOperation { ProcedureName = "RET_PROFILE" };
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
