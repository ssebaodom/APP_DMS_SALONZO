using System.Collections.Generic;
using System.Data.SqlClient;

namespace SSE.Core.Services.Helpers
{
    public static class SqlHepler
    {
        public static SqlParameter[] CreateSqlPrams(object data)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            var PropertiesResult = data.GetType().GetProperties();
            foreach (var pro in PropertiesResult)
            {
                sqlParameters.Add(new SqlParameter($"@{pro.Name}", pro.GetValue(data, null)));
            }
            return sqlParameters.ToArray();
        }
    }
}