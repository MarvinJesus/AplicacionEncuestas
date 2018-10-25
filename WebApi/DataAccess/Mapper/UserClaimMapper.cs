using DataAccess.Dao;
using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class UserClaimMapper : EntityMapper, ISqlStaments, IObjectMapper
    {
        private const string DB_COL_ID = "ID";
        private const string DB_COL_VALUE = "CLAIM_VALUE";
        private const string DB_COL_TYPE = "CLAIM_TYPE";
        private const string DB_COL_USERID = "USER_ID";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var userClaim = new UserClaim
            {
                Id = GetIntValue(row, DB_COL_ID),
                Value = GetStringValue(row, DB_COL_VALUE),
                Type = GetStringValue(row, DB_COL_TYPE),
                UserId = GetGuidValue(row, DB_COL_USERID)

            };
            return userClaim;
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
            var operation = new SqlOperation { ProcedureName = "CRE_USER_CLAIMS" };

            var userClaim = (UserClaim)entity;

            operation.AddVarcharParam(DB_COL_VALUE, userClaim.Value);
            operation.AddVarcharParam(DB_COL_TYPE, userClaim.Type);
            operation.AddGuidParam(DB_COL_USERID, userClaim.UserId);

            return operation;
        }

        public SqlOperation GetUserClaims(BaseEntity entity)
        {
            var operation = new SqlOperation { ProcedureName = "RET_USER_CLAIMS" };
            var userClaim = (UserClaim)entity;
            operation.AddGuidParam(DB_COL_USERID, userClaim.UserId);
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
