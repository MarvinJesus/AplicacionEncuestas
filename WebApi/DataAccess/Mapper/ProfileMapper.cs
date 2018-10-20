using DataAccess.Dao;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class ProfileMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "PROFILE_ID";
        private const string DB_COL_IDENTIFICATION = "IDENTIFICATION";
        private const string DB_COL_EMAIL = "EMAIL";
        private const string DB_COL_NAME = "NAME";
        private const string DB_COL_IMAGEPATH = "IMG_URL";
        //private const string DB_COL_PASSWORD = "PASSWORD";
        //private const string DB_COL_SALT = "SALT";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var Profile = new Profile
            {
                Id = GetGuidValue(row, DB_COL_ID),
                Identification = GetStringValue(row, DB_COL_IDENTIFICATION),
                Email = GetStringValue(row, DB_COL_EMAIL),
                Name = GetStringValue(row, DB_COL_NAME),
                ImagePath = GetStringValue(row, DB_COL_IMAGEPATH),
                //Password = GetBytesValue(row, DB_COL_PASSWORD),
                //Salt = GetBytesValue(row, DB_COL_SALT)
            };
            return Profile;
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
            var Profile = (Profile)entity;
            var operation = new SqlOperation { ProcedureName = "CRE_PROFILE" };

            operation.AddVarcharParam(DB_COL_IDENTIFICATION, Profile.Identification);
            operation.AddVarcharParam(DB_COL_EMAIL, Profile.Email);
            operation.AddVarcharParam(DB_COL_NAME, Profile.Name);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, Profile.ImagePath);
            //operation.AddVarBinaryParam(DB_COL_PASSWORD, Profile.Password);
            //operation.AddVarBinaryParam(DB_COL_SALT, Profile.Salt);

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
            var Profile = (Profile)entity;
            operation.AddGuidParam(DB_COL_ID, Profile.Id);
            return operation;
        }

        SqlOperation ISqlStaments.GetUpdateStatement(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
