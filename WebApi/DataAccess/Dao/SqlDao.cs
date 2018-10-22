using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Dao
{
    public class SqlDao
    {
        private const string CONNECTION_STRING = @"server=(local);database=DB_ENCUESTA;trusted_connection=yes;";
        //@"Data Source=DESKTOP-CKOCDKQ\SERVIDORSQLMSS;Initial Catalog=DB_ENCUESTA;User ID=marvin;Password=1234567890";

        private static SqlDao instance;

        private SqlDao()
        {

        }
        public static SqlDao GetInstance()
        {
            if (instance == null)
                instance = new SqlDao();

            return instance;
        }
        public int ExecuteProcedure(SqlOperation sqlOperation)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                foreach (var param in sqlOperation.Parameters)
                {
                    command.Parameters.Add(param);
                }

                conn.Open();
                return command.ExecuteNonQuery();
            }
        }
        public List<Dictionary<string, object>> ExecuteQueryProcedure(SqlOperation sqlOperation)
        {
            var lstResult = new List<Dictionary<string, object>>();

            using (var conn = new SqlConnection(CONNECTION_STRING))
            using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                foreach (var param in sqlOperation.Parameters)
                {
                    command.Parameters.Add(param);
                }

                conn.Open();
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var dict = new Dictionary<string, object>();
                        for (var lp = 0; lp < reader.FieldCount; lp++)
                        {
                            dict.Add(reader.GetName(lp), reader.GetValue(lp));
                        }
                        lstResult.Add(dict);
                    }
                }
            }

            return lstResult;
        }
    }
}
