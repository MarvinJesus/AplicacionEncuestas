using Entities_POJO;
using System.Collections.Generic;

namespace DataAccess.Mapper
{
    public class RoleMapper : EntityMapper, IObjectMapper
    {
        private const string DB_COL_ID = "ROLE_ID";
        private const string DB_COL_NAME = "ROLE_NAME";

        public BaseEntity BuildObject(Dictionary<string, object> row)
        {
            var role = new Role
            {
                Id = GetIntValue(row, DB_COL_ID),
                Name = GetStringValue(row, DB_COL_NAME)
            };
            return role;
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
    }
}
