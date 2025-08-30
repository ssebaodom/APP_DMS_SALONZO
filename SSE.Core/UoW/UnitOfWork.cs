using Microsoft.EntityFrameworkCore;
using SSE.Core.Repositories;
using SSE.Core.Services.Dapper;
using SSE.Core.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SSE.Core.UoW
{
    public class UnitOfWork<TypeDbContext> : IUnitOfWork<TypeDbContext> where TypeDbContext : DbContext
    {
        private readonly DbContext context;
        private readonly IDapperService dapperService;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(IEnumerable<DbContext> contexts, IDapperService dapperService)
        {
            this.context = contexts.FirstOrDefault(item => item.GetType() == typeof(TypeDbContext));
            this.dapperService = dapperService;
        }

        public int Save()
        {
            return this.context.SaveChanges();
        }

        public int Save(Action beforeSave)
        {
            beforeSave?.Invoke();
            return this.context.SaveChanges();
        }

        public bool Transaction(Action express, Action errors)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    express?.Invoke();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    errors?.Invoke();
                }
            }
            return false;
        }

        public IDapperService GetDapperService()
        {
            return this.dapperService;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(IRepository<T>)))
            {
                this.repositories.Add(typeof(IRepository<T>), new Repository<T>(this.context.Set<T>()));
            }
            return (IRepository<T>)this.repositories[typeof(IRepository<T>)];
        }

        public Task<int> SaveAsync()
        {
            return this.context.SaveChangesAsync();
        }

        public Task<int> SaveAsync(Action beforeSave)
        {
            beforeSave?.Invoke();
            return this.context.SaveChangesAsync();
        }

        public int ExecuteSql(string sql, object prams)
        {
            return this.context.Database.ExecuteSqlRaw(sql, SqlHepler.CreateSqlPrams(prams));
        }

        public int ExecuteSqlStore(string sql, params SqlParameter[] parameters)
        {
            return this.context.Database.ExecuteSqlRaw($"EXECUTE {sql}", parameters);
        }

        public Task<int> ExecuteSqlAsync(string sql, object prams)
        {
            return this.context.Database.ExecuteSqlRawAsync(sql, SqlHepler.CreateSqlPrams(prams));
        }

        public Task<int> ExecuteSqlStoreAsymc(string sql, params SqlParameter[] parameters)
        {
            return this.context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}