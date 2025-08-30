using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SSE.Core.Common.Constant;
using SSE.Core.Common.Constants;
using SSE.Core.Common.Entities;
using SSE.Core.Services.Caches;
using SSE.Core.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SSE.Core.Services.Dapper
{
    public class DapperService : IDapperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly DynamicParameterMap dynamicParameterMap;
        private readonly ICached cached;
        private SqlConnection connection;
        private string connectString;

        public DapperService(IConfiguration configuration,
                             IHttpContextAccessor httpContextAccessor,
                             DynamicParameterMap dynamicParameterMap,
                             ICached cached)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            this.configuration = configuration;
            this.dynamicParameterMap = dynamicParameterMap;
            this._httpContextAccessor = httpContextAccessor;
            this.cached = cached;
            SetConnectionString();
        }

        public void Dispose()
        {
            try
            {
                connection.Dispose();
            }
            catch (Exception e)
            {
            }
        }

        private void SetConnectionString()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                // get userName, hostId from token
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                if (identity.IsAuthenticated)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    string hostId = claims.FirstOrDefault(c => c.Type == CLAIM_NAMES.HOSTID).Value;
                    string userName = claims.FirstOrDefault(c => c.Type == CLAIM_NAMES.USERNAME).Value;
                    string conStr = string.Empty;

                    // get conStr lưu trong cache.
                    var account = cached.Get<UserInfoCache>(string.Concat(CORE_CACHE_KEYS.ACCOUNT, hostId, "_", userName));

                    if (account == null)
                    {
                        throw new Exception(CORE_STRINGS.USER_CACHE_NOT_FOUND);
                    }

                    // Nếu chưa có AppDbName thì dùng SysDbName
                    if (string.IsNullOrWhiteSpace(account.DbAppName))
                    {
                        conStr = this.configuration.GetConnectionString(CONFIGURATION_KEYS.CONNECTION_STRING_FORMAT);
                        conStr = string.Format(conStr, account.ServerName, account.DbSysName,
                                               account.SqlUserLogin, CryptHelper.Decrypt(account.SqlPassLogin));
                    }
                    else
                    {
                        conStr = this.configuration.GetConnectionString(CONFIGURATION_KEYS.CONNECTION_STRING_FORMAT);
                        conStr = string.Format(conStr, account.ServerName, account.DbAppName,
                                               account.SqlUserLogin, CryptHelper.Decrypt(account.SqlPassLogin));
                    }

                    this.connectString = conStr;
                }
                else // Chưa đăng nhập thì dùng chuỗi kết nối của global db.
                {
                    this.connectString = this.configuration.GetConnectionString(CONFIGURATION_KEYS.GLOBAL_CONNECTION_STRING);
                }
            }
            else // Chưa đăng nhập thì dùng chuỗi kết nối của global db.
            {
                this.connectString = this.configuration.GetConnectionString(CONFIGURATION_KEYS.GLOBAL_CONNECTION_STRING);
            }


            connection = new SqlConnection(this.connectString);
        }

        public void SetNewConnection(string conStr)
        {
            this.connection.Dispose();
            this.connection = new SqlConnection(conStr);
        }

        public DynamicParameters CreateDynamicParameters<T>(T data) where T : class
        {
            return this.dynamicParameterMap.Map<T>(data);
        }

        public DynamicParameters CreateDynamicParameters(Dictionary<string, object> data)
        {
            return this.dynamicParameterMap.Map(data);
        }

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return connection.Execute(sql, parameters, null, null, commandType);
        }

        /// <summary>
        /// Execute a command asynchronously using Task.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteAsync(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                return await connection.ExecuteAsync(sql, parameters, null, null, commandType);
            }
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell returned, as T.</returns>
        public T ExecuteScalar<T>(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return connection.ExecuteScalar<T>(sql, parameters, null, null, commandType);
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        /// <typeparam name="T"> The type to return.</typeparam>
        /// <param name="sql"> The SQL to execute.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The first cell returned, as T.</returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return await connection.ExecuteScalarAsync<T>(sql, parameters, null, null, commandType);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="sql"> The SQL to execute.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>
        ///  A sequence of data of the supplied type; if a basic type (int, string, etc) is
        ///  queried then the data from the first column in assumed, otherwise an instance
        ///  is created per row, and a direct column-name===member-name mapping is assumed
        ///  (case insensitive).
        /// </returns>
        public IEnumerable<T> Query<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {

            return connection.Query<T>(sql, parameters, null, true, null, commandType);
        }

        /// <summary>
        /// Execute a query asynchronously using Task.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="sql"> The SQL to execute.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>
        /// A sequence of data of T; if a basic type (int, string, etc) is queried then the
        /// data from the first column in assumed, otherwise an instance is created per row,
        /// and a direct column-name===member-name mapping is assumed (case insensitive).
        /// </returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return await connection.QueryAsync<T>(sql, parameters, null, null, commandType);
        }

        /// <summary>
        /// Executes a single-row query, returning the data typed as T.
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql"> The SQL to execute.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is
        /// queried then the data from the first column in assumed, otherwise an instance
        /// is created per row, and a direct column-name===member-name mapping is assumed
        /// (case insensitive).
        /// </returns>
        public T QueryFirstOrDefault<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return connection.QueryFirstOrDefault<T>(sql, parameters, null, null, commandType);
        }

        /// <summary>
        ///  Execute a single-row query asynchronously using Task.
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql"> The SQL to execute.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is
        /// queried then the data from the first column in assumed, otherwise an instance
        /// is created per row, and a direct column-name===member-name mapping is assumed
        /// (case insensitive).
        /// </returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, null, null, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sql"> The SQL to execute.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>
        /// The grid reader provides interfaces for reading multiple result sets from a Dapper query
        /// </returns>
        public GridReader QueryMultiple(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return connection.QueryMultiple(sql, parameters, null, null, commandType);
        }

        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sql"> The SQL to execute.</param>
        /// <param name="parameters">The parameters to use for this query.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>
        /// The grid reader provides interfaces for reading multiple result sets from a Dapper query
        /// </returns>
        public async Task<GridReader> QueryMultipleAsync(string sql, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return await connection.QueryMultipleAsync(sql, parameters, null, null, commandType);
        }
    }
}