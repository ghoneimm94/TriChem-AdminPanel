using PagedList;
using System.Collections.Generic;

namespace TriChem.API.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Result()
        {
            Success = false;
            Message = string.Empty;
        }
    }

    public class Result<TEntity> : Result where TEntity : class, new()
    {
        public TEntity Entity { get; set; }
    }

    public class Results<TEntity> : Result where TEntity : class, new()
    {
        public IList<TEntity> Entities { get; set; }
    }

    public class PagedResults<TEntity> : Result where TEntity : class, new()
    {
        public IPagedList<TEntity> Entities { get; set; }
        public IPagedList MetaData { get; set; }
    }

    public class SelectResults<TType> : Result
    {
        public ICollection<TType> Collection { get; set; }
    }
}