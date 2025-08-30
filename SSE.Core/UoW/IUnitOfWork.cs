using Microsoft.EntityFrameworkCore;
using SSE.Core.Repositories;
using SSE.Core.Services.Dapper;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SSE.Core.UoW
{
    public interface IUnitOfWork<TypeDbContext> where TypeDbContext : DbContext
    {
        int Save();

        Task<int> SaveAsync();

        int Save(Action beforeSave);

        Task<int> SaveAsync(Action beforeSave);

        bool Transaction(Action handle, Action errors);

        IDapperService GetDapperService();

        IRepository<T> GetRepository<T>() where T : class;

        int ExecuteSql(string sql, object prams);

        Task<int> ExecuteSqlAsync(string sql, object prams);

        int ExecuteSqlStore(string sql, params SqlParameter[] parameters);

        Task<int> ExecuteSqlStoreAsymc(string sql, params SqlParameter[] parameters);
    }
}