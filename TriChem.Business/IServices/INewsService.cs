using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.News.SearchModels;
using TriChem.Models.News.ViewModels;

namespace TriChem.Business.IServices
{
    public interface INewsService
    {
        PagedResults<NewsListVM> Get(NewsSM newsSM);
        Results<NewsListVM> Get();
        Result<NewsDetailsVM> Get(int id);
        Result<NewsDetailsVM> Add(NewsDetailsVM newsVM);
        Result Update(IEnumerable<NewsDetailsVM> news);
        Result Delete(IEnumerable<int> ids);
    }
}
