using System;
using System.Collections.Generic;
using AutoMapper;
using PagedList;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.Certificate.SearchModels;
using TriChem.Models.Certificate.ViewModels;
using TriChem.Resources.Global;
using TriChem.Resources.Certificate;
using System.Linq;

namespace TriChem.Business.Services
{
    public class CertificateService : ICertificateService
    {
        #region Services
        private readonly IRepository<Certificate> _certificateRepository;
        #endregion

        #region Constructor
        //public CertificateService(IRepository<Certificate> certificateRepository)
        public CertificateService()
        {
            _certificateRepository = new Repository<Certificate>(new TriChemEntities());
        }
        #endregion

        #region Methods
        public Result<CertificateDetailsVM> Add(CertificateDetailsVM certificateVM)
        {
            var result = _certificateRepository.AddOne(Mapper.Map<Certificate>(certificateVM), Messages.Added);
            if (result.Success)
                return new Result<CertificateDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<CertificateDetailsVM>(result.Entity) };
            return new Result<CertificateDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var result = _certificateRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<CertificateListVM> Get()
        {
            var result = _certificateRepository.GetMany(null, c => c.Id, "success",c=>c.Category);
            if (result.Success)
                return new Results<CertificateListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<CertificateListVM>>(result.Entities),
                };
            return new Results<CertificateListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<CertificateDetailsVM> Get(int id)
        {
            var result = _certificateRepository.GetOne(c => c.Id == id, "success", c => c.Category);
            if (!result.Success)
                return new Result<CertificateDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<CertificateDetailsVM> { Success = true, Entity = Mapper.Map<CertificateDetailsVM>(result.Entity) };
        }

        public PagedResults<CertificateListVM> Get(CertificateSM certificateSM)
        {
            var result = _certificateRepository.GetMany(c =>
                                                          (certificateSM.Title == null || c.Title.Contains(certificateSM.Title)) &&
                                                          (certificateSM.CategoryTitle == null || c.Category.Title.Contains(certificateSM.CategoryTitle)),
                                                          c => c.Id, certificateSM.PageNumber, certificateSM.PageSize, "success", c => c.Category);
            if (result.Success)
                return new PagedResults<CertificateListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<CertificateListVM>>(result.Entities),
                };
            return new PagedResults<CertificateListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Update(IEnumerable<CertificateDetailsVM> categories)
        {
            var result = _certificateRepository.UpdateMany(Mapper.Map<IEnumerable<Certificate>>(categories), Messages.Updated);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
