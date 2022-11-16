using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.ClientFeedback.SearchModels;
using TriChem.Models.ClientFeedback.ViewModels;

namespace TriChem.Business.IServices
{
    public interface IClientFeedbackService
    {
        PagedResults<ClientFeedbackListVM> Get(ClientFeedbackSM clientFeedbackSM);
        Results<ClientFeedbackListVM> Get();
        Result<ClientFeedbackDetailsVM> Get(int id);
        Result<ClientFeedbackDetailsVM> Add(ClientFeedbackDetailsVM clientFeedbackVM);
        Result Update(IEnumerable<ClientFeedbackDetailsVM> clientFeedbacks);
        Result Delete(IEnumerable<int> ids);
    }
}
