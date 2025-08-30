using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SSE.Core.Repositories
{
    public interface IRepository<TObject> where TObject : class
    {
        DbSet<TObject> GetDbSet();

        IQueryable<TObject> GetAll();

        List<TObject> ToList();

        Task<List<TObject>> ToListAsync();

        TObject FirstOrDefault(Expression<Func<TObject, bool>> predicate = null);

        Task<TObject> FirstOrDefaultAsync(Expression<Func<TObject, bool>> predicate = null);

        TObject Last(Expression<Func<TObject, bool>> predicate = null);

        Task<TObject> LastAsync(Expression<Func<TObject, bool>> predicate = null);

        Task<TObject> LastOrDefaultAsync(Expression<Func<TObject, bool>> predicate = null);

        IQueryable<TObject> Find(Expression<Func<TObject, bool>> predicate);

        int Count(Expression<Func<TObject, bool>> predicate = null);

        Task<int> CountAsync(Expression<Func<TObject, bool>> predicate = null);

        long LongCount(Expression<Func<TObject, bool>> predicate = null);

        Task<long> LongCountAsync(Expression<Func<TObject, bool>> predicate = null);

        Task<bool> AllAsync(Expression<Func<TObject, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<TObject, bool>> predicate);

        Task<bool> ContainsAsync(TObject item);

        IEnumerable<TObject> Distinct();

        IQueryable<TObject> Select(Expression<Func<TObject, TObject>> selector);

        IQueryable<TObject> Select(Expression<Func<TObject, int, TObject>> selector);

        IQueryable<TObject> SelectMany(Expression<Func<TObject, IEnumerable<TObject>>> selector);

        IQueryable<TObject> SelectMany(Expression<Func<TObject, int, IEnumerable<TObject>>> selector);

        IQueryable<TObject> Where(Expression<Func<TObject, bool>> predicate = null);

        IQueryable<TObject> Where(Expression<Func<TObject, int, bool>> predicate = null);

        void Insert(TObject data);

        void InsertMany(IEnumerable<TObject> datas);

        void Update(TObject data);

        void Delete(TObject data);

        void DeleteMany(IEnumerable<TObject> datas);

        bool Exits(Expression<Func<TObject, bool>> predicate);

        IQueryable<TObject> SqlRaw(string sql, object prams);

        IQueryable<TObject> StoredSqlRaw(string sql, params SqlParameter[] parameters);
    }
}