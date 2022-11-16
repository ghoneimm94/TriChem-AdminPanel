using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.Category.SearchModels;
using TriChem.Models.Category.ViewModels;

namespace TriChem.Business.IServices
{
    public interface ICategoryService
    {
        PagedResults<CategoryListVM> Get(CategorySM categorySM);
        Results<CategoryListVM> Get();
        Results<CategoryListWithChildsVM> GetWithChilds();
        Result<CategoryDetailsVM> Get(int id);
        Result<CategoryDetailsVM> Add(CategoryDetailsVM categoryVM);
        Result Update(CategoryDetailsVM categoryVM);
        Result Delete(IEnumerable<int> ids);
    }
}
