using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TriChem.DataAccess.Models;

namespace TriChem.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        #region Get        
        /// <summary>
        /// A Generic synchronous method for selecting one entity with filter parameter (where)
        /// </summary>
        /// <param name="where"></param>
        /// <param name="successMessage"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        Result<TEntity> GetOne(Expression<Func<TEntity, bool>> where, string successMessage,
            params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// A Generic asynchronous method for selecting one entity with filter parameter (where)
        /// </summary>
        /// <param name="where"></param>
        /// <param name="successMessage"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        Task<Result<TEntity>> GetOneAsync(Expression<Func<TEntity, bool>> where, string successMessage,
            params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// A Generic synchronous method for selecting many entities with an optional filter parameter (where)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="successMessage"></param>
        /// <param name="navigationProperties"></param>
        /// <returns>If where is presented it will return many entities, otherwise all entities</returns>
        PagedResults<TEntity> GetMany<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderBy,
            int pageNumber, int pageSize, string successMessage, params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// A Generic synchronous method for selecting many entities with an optional filter parameter (where)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="successMessage"></param>
        /// <param name="navigationProperties"></param>
        /// <returns>If where is presented it will return many entities, otherwise all entities</returns>
        Results<TEntity> GetMany<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderBy,
            string successMessage, params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// A Generic asynchronous method for selecting many entities with an optional filter parameter (where)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="successMessage"></param>
        /// <param name="navigationProperties"></param>
        /// <returns>If where is presented it will return many entities, otherwise all entities</returns>
        Task<PagedResults<TEntity>> GetManyAsync<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> orderBy,
            int pageNumber, int pageSize, string successMessage, params Expression<Func<TEntity, object>>[] navigationProperties);

        /// <summary>
        /// A Generic synchronous method for executing stored procedure with optional parameters
        /// </summary>
        /// <param name="successMessage"></param>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns>Paged List of Entities</returns>
        PagedResults<TEntity> ExecuteProcedure(string procedureName, int pageNumber, int pageSize,
            string successMessage, params object[] parameters);

        /// <summary>
        /// A Generic synchronous method for executing stored procedure with optional parameters
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="successMessage"></param>
        /// <param name="parameters"></param>
        /// <returns>List of Entities</returns>
        SelectResults<TEntity> ExecuteProcedure(string procedureName, string successMessage, params object[] parameters);

        /// <summary>
        /// A Generic asynchronous method for executing stored procedure with optional parameters
        /// </summary>
        /// <param name="successMessage"></param>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<PagedResults<TEntity>> ExecuteProcedureAsync(string procedureName, int pageNumber, int pageSize,
            string successMessage, params object[] parameters);

        /// <summary>
        /// A Generic method for selecting specific columns with an optional filter parameter (where)
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        SelectResults<TType> Select<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TType>> select,
            string successMessage);
        #endregion Get

        #region Create        
        /// <summary>
        /// A Generic synchronous method for inserting an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        Result<TEntity> AddOne(TEntity entity, string successMessage);

        /// <summary>
        /// A Generic asynchronous method for inserting an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Result> AddOneAsync(TEntity entity, string successMessage);

        /// <summary>
        /// A Generic synchronous method for inserting many entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        Result AddMany(IEnumerable<TEntity> entities, string successMessage);

        /// <summary>
        /// A Generic asynchronous method for inserting many entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        Task<Result> AddManyAsync(IEnumerable<TEntity> entities, string successMessage);
        #endregion Create

        #region Update        
        /// <summary>
        /// A Generic synchronous method for updating many entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        Result UpdateMany(IEnumerable<TEntity> entities, string successMessage,
            params Expression<Func<TEntity, object>>[] excludedProperties);

        /// <summary>
        /// A Generic asynchronous method for updating many entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<Result> UpdateManyAsync(IEnumerable<TEntity> entities, string successMessage);
        #endregion Update

        #region Delete        
        /// <summary>
        /// A Generic synchronous Method for deleting many entities
        /// </summary>
        /// <param name="where"></param>
        /// <param name="successMessage"></param>
        /// <returns></returns>
        Result DeleteMany(Expression<Func<TEntity, bool>> where, string successMessage);

        /// <summary>
        /// A Generic asynchronous Method for deleting many entities
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<Result> DeleteManyAsync(Expression<Func<TEntity, bool>> where, string successMessage);
        #endregion Delete

        #region Count
        /// <summary>
        /// A Generic synchronous method for counting entities
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// A Generic asynchronous method for counting entities
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> where);
        #endregion Count

        #region Exists
        /// <summary>
        /// A Generic synchronous method for checking if some entities exist or no (1 = exist, 0 = nonexist, -1 = error occured)
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int Exists(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// A Generic asynchronous method for checking if some entities exist or no (1 = exist, 0 = nonexist, -1 = error occured)
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<int> ExistsAsync(Expression<Func<TEntity, bool>> where);
        #endregion
    }
}
