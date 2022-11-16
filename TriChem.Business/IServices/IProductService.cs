using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.Product.SearchModels;
using TriChem.Models.Product.ViewModels;

namespace TriChem.Business.IServices
{
    public interface IProductService
    {
        PagedResults<ProductListVM> Get(ProductSM productSM);
        Results<ProductListVM> Get(string query, int PageNumber);
        Results<ProductListVM> Get();
        Results<ProductListVM> GetRelated(int id);
        Result<ProductDetailsVM> Get(int id);
        Result<ProductDetailsVM> Add(ProductDetailsVM productVM);
        Result Update(ProductDetailsVM product);
        Result Delete(IEnumerable<int> ids);
    }
}
