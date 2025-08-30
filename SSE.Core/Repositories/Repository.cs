using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SSE.Core.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SSE.Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> DbSet;

        public Repository(DbSet<T> DbSet)
        {
            this.DbSet = DbSet;
        }

        public DbSet<T> GetDbSet()
        {
            return this.DbSet;
        }

        public IQueryable<T> GetAll()
        {
            return this.DbSet.AsNoTracking();
        }

        /// <summary>
        /// Creates a System.Collections.Generic.List from an System.Collections.Generic.IEnumerable.
        /// </summary>
        /// <returns>A System.Collections.Generic.List that contains elements from the input sequence.</returns>
        public List<T> ToList()
        {
            return this.DbSet.ToList();
        }

        /// <summary>
        ///   Asynchronously creates a System.Collections.Generic.List`1 from an System.Linq.IQueryable`1
        ///   by enumerating it asynchronously.
        /// </summary>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task result contains a
        ///   System.Collections.Generic.List`1 that contains elements from the input sequence.
        /// </returns>
        public async Task<List<T>> ToListAsync()
        {
            return await this.DbSet.ToListAsync();
        }

        /// <summary>
        ///     Returns a new query where the change tracker will not track any of the entities
        ///     that are returned. If the entity instances are modified, this will not be detected
        ///     by the change tracker and Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///     will not persist those changes to the database.
        ///     Disabling change tracking is useful for read-only scenarios because it avoids
        ///     the overhead of setting up change tracking for each entity instance. You should
        ///     not disable change tracking if you want to manipulate entity instances and persist
        ///     those changes to the database using Microsoft.EntityFrameworkCore.DbContext.SaveChanges.
        ///     The default tracking behavior for queries can be controlled by Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.QueryTrackingBehavior.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A new query where the result set will not be tracked by the context.</returns>
        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.AsNoTracking().Where(predicate);
        }

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains
        /// no elements.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>default(TSource) if source is empty; otherwise, the first element in source.</returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? this.DbSet.AsNoTracking().FirstOrDefault() : this.DbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Asynchronously returns the first element of a sequence, or a default value if
        /// the sequence contains no elements.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains default
        /// (TSource ) if source is empty; otherwise, the first element in source.
        /// </returns>
        /// Remarks:
        ///     Multiple active operations on the same context instance are not supported. Use
        ///     'await' to ensure that any asynchronous operations have completed before calling
        ///     another method on this context.
        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? this.DbSet.AsNoTracking().FirstOrDefaultAsync() : this.DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The last element in source that passes the test specified by predicate.
        /// </returns>
        public T Last(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? this.DbSet.Last() : this.DbSet.Last(predicate);
        }

        /// <summary>
        ///  Asynchronously returns the last element of a sequence that satisfies a specified
        ///  condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   A task that represents the asynchronous operation.The task result contains the
        ///   last element in source that passes the test in predicate.
        /// </returns>
        public async Task<T> LastAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? await this.DbSet.LastAsync()
                                   : await this.DbSet.LastAsync(predicate);
        }

        /// <summary>
        /// Asynchronously returns the last element of a sequence that satisfies a specified
        /// condition or a default value if no such element is found.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///  A task that represents the asynchronous operation. The task result contains default
        ///  ( TSource ) if source is empty or if no element passes the test specified by
        ///  predicate ; otherwise, the last element in source that passes the test specified
        ///  by predicate.
        /// </returns>
        public async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? await this.DbSet.LastOrDefaultAsync()
                                     : await this.DbSet.LastOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Returns the number of elements in the specified sequence that satisfies a condition.
        /// if condition is null then count all elemens.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// The number of elements in the sequence that satisfies the condition in the predicate
        /// function.
        /// </returns>
        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? this.DbSet.Count() : this.DbSet.Count(predicate);
        }

        /// <summary>
        /// Asynchronously returns the number of elements in a sequence that satisfy a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// number of elements in the sequence that satisfy the condition in the predicate function.
        /// </returns>
        /// Remarks:
        /// Multiple active operations on the same context instance are not supported. Use
        /// 'await' to ensure that any asynchronous operations have completed before calling
        /// another method on this context.
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? await this.DbSet.CountAsync() : await this.DbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Returns an System.Int64 that represents the total number of elements in a sequence.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// The number of elements in source.
        /// </returns>
        /// Exceptions:
        ///   T:System.ArgumentNullException:
        ///     source is null.
        ///
        ///   T:System.OverflowException:
        ///     The number of elements exceeds System.Int64.MaxValue.
        public long LongCount(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? this.DbSet.LongCount() : this.DbSet.LongCount(predicate);
        }

        /// <summary>
        /// Returns an System.Int64 that represents the total number of elements in a sequence.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// The number of elements in source.
        /// </returns>
        /// Exceptions:
        ///   T:System.ArgumentNullException:
        ///     source is null.
        ///
        ///   T:System.OverflowException:
        ///     The number of elements exceeds System.Int64.MaxValue.
        public async Task<long> LongCountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? await this.DbSet.LongCountAsync() : await this.DbSet.LongCountAsync(predicate);
        }

        /// <summary>
        ///  Asynchronously determines whether all the elements of a sequence satisfy a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task result contains true
        ///   if every element of the source sequence passes the test in the specified predicate;
        ///   otherwise, false.
        /// </returns>
        public async Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.DbSet.AllAsync(predicate);
        }

        /// <summary>
        /// Asynchronously determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///  A task that represents the asynchronous operation. The task result contains true
        ///  if any elements in the source sequence pass the test in the specified predicate;
        ///  otherwise, false.
        /// </returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? await this.DbSet.AnyAsync()
                                     : await this.DbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Asynchronously determines whether a sequence contains a specified element by
        /// using the default equality comparer.
        /// </summary>
        /// <param name="item">The object to locate in the sequence.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains true
        /// if the input sequence contains the specified value; otherwise, false.
        /// </returns>
        public async Task<bool> ContainsAsync(T item)
        {
            return await this.DbSet.ContainsAsync(item);
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer
        /// to compare values.</summary>
        /// <returns>
        ///  An System.Linq.IQueryable`1 that contains distinct elements from source.
        /// </returns>
        public IEnumerable<T> Distinct()
        {
            return this.DbSet.Distinct();
        }

        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <param name="selector"> A projection function to apply to each element.</param>
        /// <returns>
        /// An System.Linq.IQueryable`1 whose elements are the result of invoking a projection
        /// function on each element of source.
        /// </returns>
        public IQueryable<T> Select(Expression<Func<T, T>> selector)
        {
            return this.DbSet.Select(selector);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form by incorporating the element's
        /// index.
        /// </summary>
        /// <param name="selector"> A projection function to apply to each element.</param>
        /// <returns>
        ///  An System.Linq.IQueryable`1 whose elements are the result of invoking a projection
        ///  function on each element of source.</returns>
        public IQueryable<T> Select(Expression<Func<T, int, T>> selector)
        {
            return this.DbSet.Select(selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an System.Collections.Generic.IEnumerable
        /// and combines the resulting sequences into one sequence.
        /// </summary>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>
        /// An System.Linq.IQueryable`1 whose elements are the result of invoking a one-to-many
        /// projection function on each element of the input sequence.
        /// </returns>
        public IQueryable<T> SelectMany(Expression<Func<T, IEnumerable<T>>> selector)
        {
            return this.DbSet.SelectMany(selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an System.Collections.Generic.IEnumerable`1
        /// and combines the resulting sequences into one sequence. The index of each source
        /// element is used in the projected form of that element.
        /// </summary>
        /// <param name="selector">
        /// A projection function to apply to each element; the second parameter of this
        /// function represents the index of the source element.
        /// </param>
        /// <returns>
        /// An System.Linq.IQueryable`1 whose elements are the result of invoking a one-to-many
        /// projection function on each element of the input sequence.
        /// </returns>
        public IQueryable<T> SelectMany(Expression<Func<T, int, IEnumerable<T>>> selector)
        {
            return this.DbSet.SelectMany(selector);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// An System.Linq.IQueryable`1 that contains elements from the input sequence that
        /// satisfy the condition specified by predicate.
        /// </returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate = null)
        {
            return this.DbSet.Where(predicate);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. Each element's index is used
        /// in the logic of the predicate function.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.
        /// the second parameter of the function represents the index of the element in the source sequence.</param>
        /// <returns>
        /// An System.Linq.IQueryable`1 that contains elements from the input sequence that
        /// satisfy the condition specified by predicate.
        /// </returns>
        public IQueryable<T> Where(Expression<Func<T, int, bool>> predicate = null)
        {
            return this.DbSet.Where(predicate);
        }

        /// <summary>
        /// Begins tracking the given entity, and any other reachable entities that are not
        /// already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        /// state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        /// is called.
        /// Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        /// state of only a single entity.
        /// </summary>
        /// <param name="data">entity:
        /// The entity to add.
        /// </param>
        /// Returns:
        ///     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        ///     The entry provides access to change tracking information and operations for the
        ///     entity.
        public void Insert(T data)
        {
            this.DbSet.Add(data);
        }

        /// <summary>
        ///    Begins tracking the given entity, and any other reachable entities that are not
        ///    already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        ///    state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///    is called.
        ///    This method is async only to allow special value generators, such as the one
        ///    used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///    to access the database asynchronously. For all other cases the non async method
        ///    should be used.
        ///    Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        ///    state of only a single entity.
        /// </summary>
        /// <param name="data">
        ///    entity:
        ///      The entity to add.
        /// </param>
        /// <returns>
        ///  Returns:
        ///     A task that represents the asynchronous Add operation. The task result contains
        ///     the Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        ///     The entry provides access to change tracking information and operations for the
        ///     entity.
        /// </returns>
        public async Task<EntityEntry<T>> InsertAsync(T data)
        {
            return await this.DbSet.AddAsync(data);
        }

        /// <summary>
        /// Begins tracking the given entities, and any other reachable entities that are
        /// not already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        /// state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        /// is called.
        /// </summary>
        /// <param name="datas">entities: The entities to add.</param>
        public void InsertMany(IEnumerable<T> datas)
        {
            this.DbSet.AddRange(datas);
        }

        /// <summary>
        ///    Begins tracking the given entities, and any other reachable entities that are
        ///    not already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        ///    state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///    is called.
        ///    This method is async only to allow special value generators, such as the one
        ///    used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        ///    to access the database asynchronously. For all other cases the non async method
        ///    should be used.
        /// </summary>
        /// <param name="datas">entities: The entities to add.</param>
        /// <Returns>
        ///     A task that represents the asynchronous operation.
        /// </Returns>
        public async void InsertManyAsync(IEnumerable<T> datas)
        {
            await this.DbSet.AddRangeAsync(datas);
        }

        /// <summary>
        ///     Begins tracking the given entity and entries reachable from the given entity
        ///     using the Microsoft.EntityFrameworkCore.EntityState.Modified state by default,
        ///     but see below for cases when a different state will be used.
        ///     Generally, no database interaction will be performed until Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///     is called.
        ///     A recursive search of the navigation properties will be performed to find reachable
        ///     entities that are not already being tracked by the context. All entities found
        ///     will be tracked by the context.
        ///     For entity types with generated keys if an entity has its primary key value set
        ///     then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Modified
        ///     state. If the primary key value is not set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        ///     state. This helps ensure new entities will be inserted, while existing entities
        ///     will be updated. An entity is considered to have its primary key value set if
        ///     the primary key property is set to anything other than the CLR default for the
        ///     property type.
        ///     For entity types without generated keys, the state set is always Microsoft.EntityFrameworkCore.EntityState.Modified.
        ///     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        ///     state of only a single entity.
        /// </summary>
        /// <param name="data">entity: The entity to update.</param>
        /// <Returns>
        ///     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry for the entity.
        ///     The entry provides access to change tracking information and operations for the
        ///     entity.
        /// </Returns>
        public void Update(T data)
        {
            this.DbSet.Update(data);
        }

        /// <summary>
        ///     Begins tracking the given entity in the Microsoft.EntityFrameworkCore.EntityState.Deleted
        ///     state such that it will be removed from the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///     is called.
        /// </summary>
        /// <param name="data">entity: The entity to remove.</param>
        /// <Returns>
        ///     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        ///     The entry provides access to change tracking information and operations for the
        ///     entity.
        /// </Returns>
        /// Remarks:
        ///     If the entity is already tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        ///     state then the context will stop tracking the entity (rather than marking it
        ///     as Microsoft.EntityFrameworkCore.EntityState.Deleted) since the entity was previously
        ///     added to the context and does not exist in the database.
        ///     Any other reachable entities that are not already being tracked will be tracked
        ///     in the same way that they would be if Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0)
        ///     was called before calling this method. This allows any cascading actions to be
        ///     applied when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
        ///     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        ///     state of only a single entity.
        public void Delete(T data)
        {
            this.DbSet.Remove(data);
        }

        /// <summary>
        ///     Begins tracking the given entities in the Microsoft.EntityFrameworkCore.EntityState.Deleted
        ///     state such that they will be removed from the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///     is called.
        /// </summary>
        /// <param name="datas">entities: The entities to remove.</param>
        /// Remarks:
        ///     If any of the entities are already tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        ///     state then the context will stop tracking those entities (rather than marking
        ///     them as Microsoft.EntityFrameworkCore.EntityState.Deleted) since those entities
        ///     were previously added to the context and do not exist in the database.
        ///     Any other reachable entities that are not already being tracked will be tracked
        ///     in the same way that they would be if Microsoft.EntityFrameworkCore.DbSet`1.AttachRange(System.Collections.Generic.IEnumerable{`0})
        ///     was called before calling this method. This allows any cascading actions to be
        ///     applied when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
        public void DeleteMany(IEnumerable<T> datas)
        {
            this.DbSet.RemoveRange(datas);
        }

        /// <summary>
        /// CHeck if Entity is exist or not.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if entity exist or false if not.</returns>
        public bool Exits(Expression<Func<T, bool>> predicate)
        {
            return FirstOrDefault(predicate) != null;
        }

        /// <summary>
        ///     Creates a LINQ query based on a raw SQL query.
        ///     If the database provider supports composing on the supplied SQL, you can compose
        ///     on top of the raw SQL query using LINQ operators -
        ///     context.Blogs.FromSqlRaw("SELECT * FROM dbo.Blogs").OrderBy(b => b.Name)
        ///     .
        ///     As with any API that accepts SQL it is important to parameterize any user input
        ///     to protect against a SQL injection attack. You can include parameter place holders
        ///     in the SQL query string and then supply parameter values as additional arguments.
        ///     Any parameter values you supply will automatically be converted to a DbParameter
        ///     -
        ///     context.Blogs.FromSqlRaw("SELECT * FROM [dbo].[SearchBlogs]({0})", userSuppliedSearchTerm)
        ///     . You can also consider using Microsoft.EntityFrameworkCore.RelationalQueryableExtensions.FromSqlInterpolated``1(Microsoft.EntityFrameworkCore.DbSet{``0},System.FormattableString)
        ///     to use interpolated string syntax to create parameters.
        ///     This overload also accepts DbParameter instances as parameter values. This allows
        ///     you to use named parameters in the SQL query string -
        ///     context.Blogs.FromSqlRaw("SELECT * FROM [dbo].[SearchBlogs]({@searchTerm})",
        ///     new SqlParameter("@searchTerm", userSuppliedSearchTerm))
        /// </summary>
        /// <param name="sql">The raw SQL query.</param>
        /// <param name="prams">The values to be assigned to parameters.</param>
        /// <returns>
        ///     An System.Linq.IQueryable`1 representing the raw SQL query.
        /// </returns>
        public IQueryable<T> SqlRaw(string sql, object prams)
        {
            return this.DbSet.FromSqlRaw(sql, SqlHepler.CreateSqlPrams(prams)).AsNoTracking();
        }

        /// <summary>
        ///     Returns a new query where the change tracker will not track any of the entities
        ///     that are returned. If the entity instances are modified, this will not be detected
        ///     by the change tracker and Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        ///     will not persist those changes to the database.
        ///     Disabling change tracking is useful for read-only scenarios because it avoids
        ///     the overhead of setting up change tracking for each entity instance. You should
        ///     not disable change tracking if you want to manipulate entity instances and persist
        ///     those changes to the database using Microsoft.EntityFrameworkCore.DbContext.SaveChanges.
        ///     The default tracking behavior for queries can be controlled by Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.QueryTrackingBehavior.
        /// </summary>
        /// <param name="sql">Store name</param>
        /// <param name="parameters">The values to be assigned to parameters.</param>
        /// <returns>
        ///     An System.Linq.IQueryable`1 representing the raw SQL query.
        /// </returns>
        public IQueryable<T> StoredSqlRaw(string sql, params SqlParameter[] parameters)
        {
            return this.DbSet.FromSqlRaw<T>($"EXECUTE {sql}", parameters).AsNoTracking();
        }
    }
}