using System;
using System.Data.SqlClient;
using System.Text;

namespace SSE.Core.Common.Factories
{
    public class ExceptionFactory
    {
        public static Exception CreateException(int code, Exception ex)
        {
            if (ex == null)
            {
                ex = new Exception();
            }
            ex.Data["code"] = code;
            return ex;
        }

        public static StringBuilder SqlExceptionMessage(SqlException ex)
        {
            StringBuilder sqlErrorMessages = new StringBuilder("Sql Exception:\n");

            foreach (SqlError error in ex.Errors)
            {
                sqlErrorMessages.AppendFormat("Mesage: {0}\n", error.Message)
                    .AppendFormat("Severity level: {0}\n", error.Class)
                    .AppendFormat("State: {0}\n", error.State)
                    .AppendFormat("Number: {0}\n", error.Number)
                    .AppendFormat("Procedure: {0}\n", error.Procedure)
                    .AppendFormat("Source: {0}\n", error.Source)
                    .AppendFormat("LineNumber: {0}\n", error.LineNumber)
                    .AppendFormat("Server: {0}\n", error.Server)
                    .AppendLine(new string('-', error.Message.Length + 7));
            }

            return sqlErrorMessages;
        }
    }
}