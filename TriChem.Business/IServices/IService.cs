using System.Collections.Generic;
using TriChem.Business.Models;

namespace TriChem.Business.IServices
{
    public interface IService<EntityListVM, EntityListWithChildsVM, EntityDetailsVM, EntitySM> 
        where EntityListVM : class, new()
        where EntityListWithChildsVM : class, new()
        where EntityDetailsVM : class, new()
    {
        PagedResults<EntityListVM> Get(EntitySM categorySM);
        Results<EntityListVM> Get();
        Results<EntityListWithChildsVM> GetWithChilds();
        Result<EntityDetailsVM> Get(int id);
        Result<EntityDetailsVM> Add(EntityDetailsVM categoryVM);
        Result Update(IEnumerable<EntityDetailsVM> categories);
        Result Delete(IEnumerable<int> ids);
    }
}
