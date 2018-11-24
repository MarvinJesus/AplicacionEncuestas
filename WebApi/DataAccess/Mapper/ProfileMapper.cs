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
        private const string DB_COL_USERID = "USER_ID";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var Profile = new Profile
            {
                Id = GetGuidValue(row, DB_COL_ID),
                Identification = GetStringValue(row, DB_COL_IDENTIFICATION),
                Email = GetStringValue(row, DB_COL_EMAIL),
                Name = GetStringValue(row, DB_COL_NAME),
                ImagePath = GetStringValue(row, DB_COL_IMAGEPATH),
                UserId = GetGuidValue(row, DB_COL_USERID)
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
            operation.AddGuidParam(DB_COL_USERID, Profile.UserId);

            return operation;
        }

        public SqlOperation GetEditPicture(BaseEntity entity)
        {
            var profile = (Profile)entity;
            var operation = new SqlOperation { ProcedureName = "UPD_PROFILE_PICTURE" };

            operation.AddGuidParam(DB_COL_USERID, profile.UserId);
            operation.AddVarcharParam(DB_COL_IMAGEPATH, profile.ImagePath);

            return operation;
        }

        public SqlOperation GetRetrieveUserRoles(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_ROLE_BY_USER" };
            var profile = (Profile)entity;
            operation.AddGuidParam(DB_COL_USERID, profile.UserId);

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
            var operation = new SqlOperation { ProcedureName = "RET_PROFILE_BY_USER_ID" };
            var Profile = (Profile)entity;
            operation.AddGuidParam(DB_COL_USERID, Profile.UserId);
            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            var Profile = (Profile)entity;
            var operation = new SqlOperation { ProcedureName = "UPD_PROFILE" };

            operation.AddVarcharParam(DB_COL_IDENTIFICATION, Profile.Identification);
            operation.AddVarcharParam(DB_COL_EMAIL, Profile.Email);
            operation.AddVarcharParam(DB_COL_NAME, Profile.Name);
            operation.AddGuidParam(DB_COL_USERID, Profile.UserId);

            return operation;
        }
    }
}
