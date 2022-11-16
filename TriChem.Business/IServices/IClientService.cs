using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.Client.SearchModels;
using TriChem.Models.Client.ViewModels;

namespace TriChem.Business.IServices
{
    public interface IClientService
    {
        PagedResults<ClientListVM> Get(ClientSM clientSM);
        Results<ClientListVM> Get();
        Result<ClientDetailsVM> Get(int id);
        Result<ClientDetailsVM> Add(ClientDetailsVM clientVM);
        Result Update(IEnumerable<ClientDetailsVM> clients);
        Result Delete(IEnumerable<int> ids);
    }
}
