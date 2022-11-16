using AutoMapper;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.Category.SearchModels;
using TriChem.Models.Category.ViewModels;
using TriChem.Resources.Category;
using TriChem.Resources.Global;

namespace TriChem.Business.Services
{
    public class CategoryService : ICategoryService
    {
        #region Services
        private readonly IRepository<Category> _categoryRepository;
        private readonly TriChemEntities _db;
        #endregion

        #region Constructor
        //public CategoryService(IRepository<Category> categoryRepository)
        public CategoryService()
        {
            _categoryRepository = new Repository<Category>(new TriChemEntities());
            _db = new TriChemEntities();
        }
        #endregion

        #region Methods        
        public Result<CategoryDetailsVM> Add(CategoryDetailsVM categoryVM)
        {
            var entity = Mapper.Map<Category>(categoryVM);
            foreach (var item in categoryVM.ImageURLs)
            {
                entity.CategoryImage.Add(new CategoryImage
                {
                    ImageURL = item
                });
            }
            var result = _categoryRepository.AddOne(entity, Messages.Added);
            if (result.Success)
                return new Result<CategoryDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<CategoryDetailsVM>(result.Entity) };
            return new Result<CategoryDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var categories = _categoryRepository.GetMany(c => ids.Contains(c.Id), c => c.Id, "success");
            foreach (var category in categories.Entities)
                if (category.Product.Count > 0)
                    return new Result { Message = Messages.HaveProducts };

            var result = _categoryRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<CategoryListVM> Get()
        {
            var result = _categoryRepository.GetMany(null, c => c.Id, "success", c => c.CategoryImage);
            if (result.Success)
                return new Results<CategoryListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<CategoryListVM>>(result.Entities),
                };
            return new Results<CategoryListVM> { Message = ErrorMessages.GeneralError };
        }

        public Results<CategoryListWithChildsVM> GetWithChilds()
        {
            var result = _categoryRepository.GetMany(null, c => c.Id, "success", c => c.Product);
            if (result.Success)
            {
                result.Entities.ToList().ForEach(e =>
                {
                    e.Product = e.Product.OrderBy(p => p.Index).ToList();
                });
                return new Results<CategoryListWithChildsVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<CategoryListWithChildsVM>>(result.Entities),
                };
            }
            return new Results<CategoryListWithChildsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<CategoryDetailsVM> Get(int id)
        {
            var result = _categoryRepository.GetOne(c => c.Id == id, "success", c => c.CategoryImage);
            if (!result.Success)
                return new Result<CategoryDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<CategoryDetailsVM> { Success = true, Entity = Mapper.Map<CategoryDetailsVM>(result.Entity) };
        }

        public PagedResults<CategoryListVM> Get(CategorySM categorySM)
        {
            var result = _categoryRepository.GetMany(c => (categorySM.Id == 0 || c.Id == categorySM.Id) &&
                                                          (categorySM.Title == null || c.Title.Contains(categorySM.Title)) &&
                                                          (categorySM.Description == null || c.Description.Contains(categorySM.Description)),
                                                          c => c.Id, categorySM.PageNumber, categorySM.PageSize, "success");
            if (result.Success)
                return new PagedResults<CategoryListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<CategoryListVM>>(result.Entities),
                };
            return new PagedResults<CategoryListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Update(CategoryDetailsVM categoryVM)
        {
            var entity = Mapper.Map<Category>(categoryVM);
            var categoryImage = new List<CategoryImage>();
            foreach (var item in categoryVM.ImageURLs)
            {
                categoryImage.Add(new CategoryImage
                {
                    CategoryId = entity.Id,
                    ImageURL = item
                });
            }

            var result = _categoryRepository.UpdateMany(new List<Category> { entity }, Messages.Updated);

            _db.CategoryImage.AddRange(categoryImage);
            _db.SaveChanges();
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
