using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class UserMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_USERID = "USER_ID";
        private const string DB_COL_PASSWORD = "PASSWORD";
        private const string DB_COL_SALT = "SALT";
        private const string DB_COL_USERNAME = "USERNAME";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var user = new User
            {
                UserId = GetGuidValue(row, DB_COL_USERID),
                Password = GetBytesValue(row, DB_COL_PASSWORD),
                Salt = GetBytesValue(row, DB_COL_SALT)
            };
            return user;
        }

        public List<BaseEntity> BuildObjects(List<Dictionary<string, object>> lstRows)
        {
            var lstResult = new List<BaseEntity>();

            foreach (var row in lstRows)
            {
                lstResult.Add(BuildObject(row));
            }
            return lstResult;
        }

        public SqlOperation GetCreateStatement(BaseEntity entity)
        {
            var user = (User)entity;

            var operation = new SqlOperation()
            {
                ProcedureName = "CRE_USER",
            };

            operation.AddVarcharParam(DB_COL_USERNAME, user.Username);
            operation.AddVarBinaryParam(DB_COL_PASSWORD, user.Password);
            operation.AddVarBinaryParam(DB_COL_SALT, user.Salt);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public SqlOperation GetRetriveAllStatement()
        {
            throw new System.NotImplementedException();
        }

        public SqlOperation GetRetriveStatement(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public SqlOperation GetUpdateStatement(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
