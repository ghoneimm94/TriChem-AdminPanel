using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.Category.SearchModels;
using TriChem.Models.Category.ViewModels;

namespace TriChem.API.Controllers
{
    public class CategoryController : ApiController
    {
        #region Services
        private readonly ICategoryService _categoryService;
        //private readonly IService<CategoryListVM, CategoryListWithChildsVM, CategoryDetailsVM, CategorySM> _categoryService;

        #endregion

        #region Constructor
        //public CategoryController(IService<CategoryListVM, CategoryListWithChildsVM, CategoryDetailsVM, CategorySM> categoryService)
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public Results<CategoryListVM> Get()
        {
            var result = _categoryService.Get();
            if (!result.Success)
                return new Results<CategoryListVM> { Message = result.Message };
            return new Results<CategoryListVM>
            {
                Success = true,
                Entities = result.Entities
            };
        }

        [HttpGet, Route("api/Category/GetWithProducts")]
        public Results<CategoryListWithChildsVM> GetWithChilds()
        {
            var result = _categoryService.GetWithChilds();
            if (!result.Success)
                return new Results<CategoryListWithChildsVM> { Message = result.Message };
            return new Results<CategoryListWithChildsVM>
            {
                Success = true,
                Entities = result.Entities
            };
        }

        // GET: api/Category/5
        public Result<CategoryDetailsVM> Get(int id)
        {
            var result = _categoryService.Get(id);
            if (!result.Success)
                return new Result<CategoryDetailsVM> { Message = result.Message };
            return new Result<CategoryDetailsVM> { Success = true, Entity = result.Entity };
        }

        // POST: api/Category
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Category/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Category/5
        public void Delete(int id)
        {
        }
        #endregion
    }
}