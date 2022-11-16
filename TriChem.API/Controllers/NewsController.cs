using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.News.SearchModels;
using TriChem.Models.News.ViewModels;

namespace TriChem.API.Controllers
{
    public class NewsController : ApiController
    {
        #region Services
        private readonly INewsService _newsService;
        #endregion

        #region Constructor
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public PagedResults<NewsListVM> Get([FromUri]NewsSM newsSM)
        {
            var result = _newsService.Get(newsSM);
            if (!result.Success)
                return new PagedResults<NewsListVM> { Message = result.Message };
            return new PagedResults<NewsListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            };
        }

        // GET: api/Certificate/5
        public Result<NewsDetailsVM> Get(int id)
        {
            var result = _newsService.Get(id);
            if (!result.Success)
                return new Result<NewsDetailsVM> { Message = result.Message };
            return new Result<NewsDetailsVM> { Success = true, Entity = result.Entity };
        }

        // POST: api/Certificate
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Certificate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Certificate/5
        public void Delete(int id)
        {
        }
        #endregion
    }
}
