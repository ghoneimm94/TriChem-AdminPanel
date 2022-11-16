using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TriChem.DataAccess.Models;

namespace TriChem.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        #region Services
        private readonly DbContext contextFactory;
        #endregion

        #region Constructor
        public Repository(DbContext _contextFactory)
        {
            //contextFactory = _contextFactory ?? throw new NullReferenceException();
            if (_contextFactory == null)
                throw new NullReferenceException();

            contextFactory = _contextFactory;
            contextFactory.Configuration.LazyLoadingEnabled = false;
        }
        #endregion

        #region Methods        
        #region Get
        public Result<TEntity> GetOne(Expression<Func<TEntity, bool>> where, string successMessage,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            Result<TEntity> result = new Result<TEntity>();
            TEntity entity = null;

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                if (navigationProperties != null)
                    foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include(navigationProperty);

                entity = dbQuery.AsNoTracking().Where(where).FirstOrDefault();

                if (entity != null)
                    result.Entity = entity;

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public async Task<Result<TEntity>> GetOneAsync(Expression<Func<TEntity, bool>> where, string successMessage,
            params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            Result<TEntity> result = new Result<TEntity>();
            TEntity entity = null;

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                if (navigationProperties != null)
                    foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include(navigationProperty);

                entity = await dbQuery.AsNoTracking().Where(where).FirstOrDefaultAsync();

                if (entity != null)
                    result.Entity = entity;

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public PagedResults<TEntity> GetMany<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderBy,
            int pageNumber, int pageSize, string successMessage, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            PagedResults<TEntity> result = new PagedResults<TEntity>();
            IPagedList<TEntity> entities = null;

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                if (navigationProperties != null)
                    foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include(navigationProperty);

                entities = (where != null) ? dbQuery.AsNoTracking().Where(where).OrderByDescending(orderBy).ToPagedList(pageNumber, pageSize) :
                             dbQuery.AsNoTracking().OrderByDescending(orderBy).ToPagedList(pageNumber, pageSize);

                if (entities != null)
                    result.Entities = entities;

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public Results<TEntity> GetMany<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderBy, string successMessage, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            Results<TEntity> result = new Results<TEntity>();
            IList<TEntity> entities = null;

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                if (navigationProperties != null)
                    foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include(navigationProperty);

                entities = (where != null) ?
                    dbQuery.AsNoTracking().Where(where).OrderByDescending(orderBy).ToList() :
                    dbQuery.AsNoTracking().OrderByDescending(orderBy).ToList();

                if (entities != null)
                    result.Entities = entities;

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public async Task<PagedResults<TEntity>> GetManyAsync<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderBy,
            int pageNumber, int pageSize, string successMessage, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            PagedResults<TEntity> result = new PagedResults<TEntity>();
            IList<TEntity> entities = null;

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                if (navigationProperties != null)
                    foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

                entities = (where != null) ?
                    await dbQuery.AsNoTracking().Where(where).OrderByDescending(orderBy).ToListAsync() :
                    await dbQuery.AsNoTracking().OrderByDescending(orderBy).ToListAsync();

                if (entities != null)
                    result.Entities = entities.ToPagedList(pageNumber, pageSize);

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public PagedResults<TEntity> ExecuteProcedure(string procedureName, int pageNumber, int pageSize,
            string successMessage, params object[] parameters)
        {
            PagedResults<TEntity> result = new PagedResults<TEntity>();
            IPagedList<TEntity> entities = null;

            try
            {
                DbRawSqlQuery<TEntity> dbQuery = contextFactory.Database.SqlQuery<TEntity>(procedureName, parameters);

                entities = dbQuery.ToPagedList(pageNumber, pageSize);

                if (entities != null)
                    result.Entities = entities;

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public SelectResults<TEntity> ExecuteProcedure(string procedureName, string successMessage, params object[] parameters)
        {
            SelectResults<TEntity> result = new SelectResults<TEntity>();
            ICollection<TEntity> collection = null;

            try
            {
                DbRawSqlQuery<TEntity> dbQuery = contextFactory.Database.SqlQuery<TEntity>(procedureName, parameters);

                collection = dbQuery.ToList();

                if (collection != null)
                    result.Collection = collection;

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public async Task<PagedResults<TEntity>> ExecuteProcedureAsync(string procedureName, int pageNumber, int pageSize,
            string successMessage, params object[] parameters)
        {
            PagedResults<TEntity> result = new PagedResults<TEntity>();
            IList<TEntity> entities = null;

            try
            {
                DbSqlQuery<TEntity> dbQuery = contextFactory.Set<TEntity>().SqlQuery(procedureName, parameters);

                entities = await dbQuery.AsNoTracking().ToListAsync();

                if (entities != null)
                    result.Entities = entities.ToPagedList(pageNumber, pageSize);

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public SelectResults<TType> Select<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TType>> select,
            string successMessage)
        {
            SelectResults<TType> result = new SelectResults<TType>();
            ICollection<TType> collection = null;

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                collection = (where != null) ?
                    dbQuery.AsNoTracking().Where(where).Select(select).ToList() :
                    dbQuery.AsNoTracking().Select(select).ToList();

                if (collection != null)
                    result.Collection = collection;

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }
        #endregion Get

        #region Create        
        public Result<TEntity> AddOne(TEntity item, string successMessage)
        {
            var result = new Result<TEntity>();

            try
            {
                result.Entity = contextFactory.Set<TEntity>().Add(item);
                contextFactory.SaveChanges();
                result.Success = true;
                result.Message = successMessage;

                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public async Task<Result> AddOneAsync(TEntity item, string successMessage)
        {
            var result = new Result();

            try
            {
                contextFactory.Set<TEntity>().Add(item);
                await contextFactory.SaveChangesAsync();

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public Result AddMany(IEnumerable<TEntity> entities, string successMessage)
        {
            var result = new Result();

            try
            {
                if (entities.Count() > 0)
                {
                    contextFactory.Set<TEntity>().AddRange(entities);
                    contextFactory.SaveChanges();

                    result.Success = true;
                    result.Message = successMessage;
                }
                else
                {
                    result.Success = false;
                    result.Message = "There are no entities to Add!";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public async Task<Result> AddManyAsync(IEnumerable<TEntity> entities, string successMessage)
        {
            var result = new Result();

            try
            {
                if (entities.Count() > 0)
                {
                    contextFactory.Set<TEntity>().AddRange(entities);
                    await contextFactory.SaveChangesAsync();

                    result.Success = true;
                    result.Message = successMessage;
                }
                else
                {
                    result.Success = false;
                    result.Message = "There are no entities to Add!";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }
        #endregion Create

        #region Update        
        public Result UpdateMany(IEnumerable<TEntity> entities, string successMessage,
            params Expression<Func<TEntity, object>>[] excludedProperties)
        {
            var result = new Result();

            try
            {
                if (entities.Count() > 0)
                {
                    foreach (var entity in entities)
                    {
                        var ss = contextFactory.Entry(entity).State;
                        //var exist = contextFactory.Set<TEntity>().Local.Any(e => e == entity);
                        if (contextFactory.Entry(entity).State == EntityState.Detached)
                            contextFactory.Entry(entity).State = EntityState.Modified;
                        else
                            contextFactory.Entry(entity).CurrentValues.SetValues(entity);

                        foreach (var property in excludedProperties)
                            //contextFactory.Entry(entity).Collection(property.Name)
                            contextFactory.Entry(entity).Property(property).IsModified = false;
                    }

                    contextFactory.SaveChanges();

                    result.Success = true;
                    result.Message = successMessage;
                }
                else
                {
                    result.Success = false;
                    result.Message = "There are no entities to modify!";
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public async Task<Result> UpdateManyAsync(IEnumerable<TEntity> entities, string successMessage)
        {
            var result = new Result();

            try
            {
                if (entities.Count() > 0)
                {
                    foreach (var entity in entities)
                        contextFactory.Entry(entity).State = EntityState.Modified;

                    await contextFactory.SaveChangesAsync();

                    result.Success = true;
                    result.Message = successMessage;
                }
                else
                {
                    result.Success = false;
                    result.Message = "There no entities to modify!";
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }
        #endregion Update

        #region Delete                       
        public Result DeleteMany(Expression<Func<TEntity, bool>> where, string successMessage)
        {
            var result = new Result();

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                var entities = dbQuery.Where(where);
                var x = contextFactory.Set<TEntity>().RemoveRange(entities);
                contextFactory.SaveChanges();

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }

        public async Task<Result> DeleteManyAsync(Expression<Func<TEntity, bool>> where, string successMessage)
        {
            var result = new Result();

            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                var entities = await dbQuery.AsNoTracking().Where(where).ToListAsync();

                foreach (var entity in entities)
                {
                    var entry = contextFactory.Entry(entity);
                    if (entry.State == EntityState.Detached)
                        contextFactory.Set<TEntity>().Attach(entity);
                }

                contextFactory.Set<TEntity>().RemoveRange(entities);
                await contextFactory.SaveChangesAsync();

                result.Success = true;
                result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ErrorCode = ex.HResult;
                return result;
            }
        }
        #endregion Delete

        #region Count        
        public long Count(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                return (where != null) ?
                    dbQuery.AsNoTracking().Where(where).Count() :
                    dbQuery.AsNoTracking().Count();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                IQueryable<TEntity> dbQuery = contextFactory.Set<TEntity>();

                return (where != null) ?
                    await dbQuery.AsNoTracking().Where(where).CountAsync() :
                    await dbQuery.AsNoTracking().CountAsync();
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion Count

        #region Exists        
        public int Exists(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                var count = Count(where);
                return (count > 0) ? 1 : 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<int> ExistsAsync(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                var count = await CountAsync(where);
                return (count > 0) ? 1 : 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion
        #endregion
    }
}
