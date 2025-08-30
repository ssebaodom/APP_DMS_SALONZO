using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.Core.Services.Dapper
{
    public interface IDapperService : IDisposable
    {
        void SetNewConnection(string conStr);

        DynamicParameters CreateDynamicParameters<T>(T data) where T : class;

        DynamicParameters CreateDynamicParameters(Dictionary<string, object> data);

        int Execute(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<int> ExecuteAsync(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure);

        T ExecuteScalar<T>(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<T> ExecuteScalarAsync<T>(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure);

        T QueryFirstOrDefault<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);

        IEnumerable<T> Query<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);

        GridReader QueryMultiple(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);

        Task<GridReader> QueryMultipleAsync(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
    }
}