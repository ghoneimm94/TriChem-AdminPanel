using AutoMapper;
using PagedList;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.CustomerCertificate.SearchModels;
using TriChem.Models.CustomerCertificate.ViewModels;
using TriChem.Resources.Global;
using TriChem.Resources.CustomerCertificate;
using System.Linq;
using System.Collections.Generic;

namespace TriChem.Business.Services
{
    public class CustomerCertificateService : ICustomerCertificateService
    {
        #region Services
        private readonly IRepository<CustomerCertificate> _customerCertificateRepository;
        #endregion

        #region Constructor
        //public CustomerCertificateService(IRepository<CustomerCertificate> customerCertificateRepository)
        public CustomerCertificateService()
        {
            _customerCertificateRepository = new Repository<CustomerCertificate>(new TriChemEntities());
        }
        #endregion

        #region Methods
        public Result<CustomerCertificateDetailsVM> Add(CustomerCertificateDetailsVM customerCertificateVM)
        {
            var result = _customerCertificateRepository.AddOne(Mapper.Map<CustomerCertificate>(customerCertificateVM), Messages.Added);
            if (result.Success)
                return new Result<CustomerCertificateDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<CustomerCertificateDetailsVM>(result.Entity) };
            return new Result<CustomerCertificateDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var result = _customerCertificateRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<CustomerCertificateListVM> Get()
        {
            var result = _customerCertificateRepository.GetMany(null, c => c.Id, "success");
            if (result.Success)
                return new Results<CustomerCertificateListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<CustomerCertificateListVM>>(result.Entities),
                };
            return new Results<CustomerCertificateListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<CustomerCertificateDetailsVM> Get(int id)
        {
            var result = _customerCertificateRepository.GetOne(c => c.Id == id, "success");
            if (!result.Success)
                return new Result<CustomerCertificateDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<CustomerCertificateDetailsVM> { Success = true, Entity = Mapper.Map<CustomerCertificateDetailsVM>(result.Entity) };
        }

        public PagedResults<CustomerCertificateListVM> Get(CustomerCertificateSM customerCertificateSM)
        {
            var result = _customerCertificateRepository.GetMany(c =>
                                                          (customerCertificateSM.Title == null || c.Title.Contains(customerCertificateSM.Title) || c.Title_Ar.Contains(customerCertificateSM.Title)),
                                                          c => c.Id, customerCertificateSM.PageNumber, customerCertificateSM.PageSize, "success");
            if (result.Success)
                return new PagedResults<CustomerCertificateListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<CustomerCertificateListVM>>(result.Entities),
                };
            return new PagedResults<CustomerCertificateListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Update(IEnumerable<CustomerCertificateDetailsVM> categories)
        {
            var result = _customerCertificateRepository.UpdateMany(Mapper.Map<IEnumerable<CustomerCertificate>>(categories), Messages.Updated);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
