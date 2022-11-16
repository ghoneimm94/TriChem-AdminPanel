using AutoMapper;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.ClientFeedback.SearchModels;
using TriChem.Models.ClientFeedback.ViewModels;
using TriChem.Resources.ClientFeedback;
using TriChem.Resources.Global;

namespace TriChem.Business.Services
{
    public class ClientFeedbackService : IClientFeedbackService
    {
        #region Services
        private readonly IRepository<ClientFeedback> _clientFeedbackRepository;
        #endregion

        #region Constructor
        //public ClientFeedbackService(IRepository<ClientFeedback> clientFeedbackRepository)
        public ClientFeedbackService()
        {
            _clientFeedbackRepository = new Repository<ClientFeedback>(new TriChemEntities());
        }
        #endregion

        #region Methods
        public Result<ClientFeedbackDetailsVM> Add(ClientFeedbackDetailsVM clientFeedbackVM)
        {
            var result = _clientFeedbackRepository.AddOne(Mapper.Map<ClientFeedback>(clientFeedbackVM), Messages.Added);
            if (result.Success)
                return new Result<ClientFeedbackDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<ClientFeedbackDetailsVM>(result.Entity) };
            return new Result<ClientFeedbackDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var result = _clientFeedbackRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<ClientFeedbackListVM> Get()
        {
            var result = _clientFeedbackRepository.GetMany(null, c => c.Id, "success");
            if (result.Success)
                return new Results<ClientFeedbackListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<ClientFeedbackListVM>>(result.Entities),
                };
            return new Results<ClientFeedbackListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<ClientFeedbackDetailsVM> Get(int id)
        {
            var result = _clientFeedbackRepository.GetOne(c => c.Id == id, "success");
            if (!result.Success)
                return new Result<ClientFeedbackDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<ClientFeedbackDetailsVM> { Success = true, Entity = Mapper.Map<ClientFeedbackDetailsVM>(result.Entity) };
        }

        public PagedResults<ClientFeedbackListVM> Get(ClientFeedbackSM clientFeedbackSM)
        {
            var result = _clientFeedbackRepository.GetMany(c =>
                                                          (clientFeedbackSM.ClientName == null || c.ClientName.Contains(clientFeedbackSM.ClientName) || c.ClientName_Ar.Contains(clientFeedbackSM.ClientName)) &&
                                                          (clientFeedbackSM.ClientPosition == null || c.ClientPosition.Contains(clientFeedbackSM.ClientPosition) || c.ClientPosition_Ar.Contains(clientFeedbackSM.ClientPosition)) &&
                                                          (clientFeedbackSM.Message == null || c.Message.Contains(clientFeedbackSM.Message) || c.Message_Ar.Contains(clientFeedbackSM.Message)),
                                                          c => c.Id, clientFeedbackSM.PageNumber, clientFeedbackSM.PageSize, "success");
            if (result.Success)
                return new PagedResults<ClientFeedbackListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<ClientFeedbackListVM>>(result.Entities),
                };
            return new PagedResults<ClientFeedbackListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Update(IEnumerable<ClientFeedbackDetailsVM> categories)
        {
            var result = _clientFeedbackRepository.UpdateMany(Mapper.Map<IEnumerable<ClientFeedback>>(categories), Messages.Updated);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
