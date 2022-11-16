using AutoMapper;
using System.Collections.Generic;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.Client.ViewModels;
using TriChem.Resources.Client;
using TriChem.Resources.Global;
using TriChem.Models.Client.SearchModels;
using System.Linq;
using PagedList;

namespace TriChem.Business.Services
{
    public class ClientService : IClientService
    {
        #region Services
        private readonly IRepository<Client> _clientRepository;
        #endregion

        #region Constructor
        //public ClientService(IRepository<Client> clientRepository)
        public ClientService()
        {
            _clientRepository = new Repository<Client>(new TriChemEntities());
        }
        #endregion

        #region Methods
        public Result<ClientDetailsVM> Add(ClientDetailsVM clientVM)
        {
            var result = _clientRepository.AddOne(Mapper.Map<Client>(clientVM), Messages.Added);
            if (result.Success)
                return new Result<ClientDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<ClientDetailsVM>(result.Entity) };
            return new Result<ClientDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var result = _clientRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<ClientListVM> Get()
        {
            var result = _clientRepository.GetMany(null, c => c.Id, "success");
            if (result.Success)
                return new Results<ClientListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<ClientListVM>>(result.Entities),
                };
            return new Results<ClientListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<ClientDetailsVM> Get(int id)
        {
            var result = _clientRepository.GetOne(c => c.Id == id, "success");
            if (!result.Success)
                return new Result<ClientDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<ClientDetailsVM> { Success = true, Entity = Mapper.Map<ClientDetailsVM>(result.Entity) };
        }

        public PagedResults<ClientListVM> Get(ClientSM clientSM)
        {
            var result = _clientRepository.GetMany(c =>
                                                          (clientSM.Title == null || c.Title.Contains(clientSM.Title) || c.Title_Ar.Contains(clientSM.Title)),
                                                          c => c.Id, clientSM.PageNumber, clientSM.PageSize, "success");
            if (result.Success)
                return new PagedResults<ClientListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<ClientListVM>>(result.Entities),
                };
            return new PagedResults<ClientListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Update(IEnumerable<ClientDetailsVM> categories)
        {
            var result = _clientRepository.UpdateMany(Mapper.Map<IEnumerable<Client>>(categories), Messages.Updated);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
