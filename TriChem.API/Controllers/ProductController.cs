using System.Linq;
using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.Product.SearchModels;
using TriChem.Models.Product.ViewModels;

namespace TriChem.API.Controllers
{
    public class ProductController : ApiController
    {
        #region Services
        private readonly IProductService _productService;
        #endregion

        #region Constructor
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public PagedResults<ProductListVM> Get([FromUri]ProductSM productSM)
        {
            var result = _productService.Get(productSM);
            if (!result.Success)
                return new PagedResults<ProductListVM> { Message = result.Message };
            return new PagedResults<ProductListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            };
        }

        [HttpGet, Route("api/Product/GetAny")]
        public Results<ProductListVM> Get(string query, int PageNumber = 1)
        {
            var result = _productService.Get(query, PageNumber);
            if (!result.Success)
                return new Results<ProductListVM> { Message = result.Message };
            return new Results<ProductListVM>
            {
                Success = true,
                Entities = result.Entities
            };
        }

        [HttpGet, Route("api/Product/GetRelated")]
        public Results<ProductListVM> GetRelated(int id)
        {
            var result = _productService.GetRelated(id);
            if (!result.Success)
                return new Results<ProductListVM> { Message = result.Message };
            return new Results<ProductListVM>
            {
                Success = true,
                Entities = result.Entities
            };
        }

        // GET: api/Product/5
        public Result<ProductDetailsVM> Get(int id)
        {
            var result = _productService.Get(id);
            if (!result.Success)
                return new Result<ProductDetailsVM> { Message = result.Message };
            return new Result<ProductDetailsVM> { Success = true, Entity = result.Entity };
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
