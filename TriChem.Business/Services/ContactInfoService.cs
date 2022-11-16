using AutoMapper;
using System.Collections.Generic;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.ContactInfo.SearchModels;
using TriChem.Models.ContactInfo.ViewModels;
using TriChem.Resources.Global;
using TriChem.Resources.ContactInfo;
using PagedList;
using System.Linq;

namespace TriChem.Business.Services
{
    public class ContactInfoService : IContactInfoService
    {
        #region Services
        private readonly IRepository<ContactInfo> _contactInfoRepository;
        #endregion

        #region Constructor
        //public ContactInfoService(IRepository<ContactInfo> contactInfoRepository)
        public ContactInfoService()
        {
            _contactInfoRepository = new Repository<ContactInfo>(new TriChemEntities());
        }
        #endregion

        #region Methods
        public Result<ContactInfoDetailsVM> Add(ContactInfoDetailsVM contactInfoVM)
        {
            var result = _contactInfoRepository.AddOne(Mapper.Map<ContactInfo>(contactInfoVM), Messages.Added);
            if (result.Success)
                return new Result<ContactInfoDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<ContactInfoDetailsVM>(result.Entity) };
            return new Result<ContactInfoDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var result = _contactInfoRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<ContactInfoListVM> Get()
        {
            var result = _contactInfoRepository.GetMany(null, c => c.Id, "success");
            if (result.Success)
                return new Results<ContactInfoListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<ContactInfoListVM>>(result.Entities),
                };
            return new Results<ContactInfoListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<ContactInfoDetailsVM> Get(int id)
        {
            var result = _contactInfoRepository.GetOne(c => c.Id == id, "success");
            if (!result.Success)
                return new Result<ContactInfoDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<ContactInfoDetailsVM> { Success = true, Entity = Mapper.Map<ContactInfoDetailsVM>(result.Entity) };
        }

        public PagedResults<ContactInfoListVM> Get(ContactInfoSM contactInfoSM)
        {
            var result = _contactInfoRepository.GetMany(c =>
                                                          (contactInfoSM.Code == null || c.Code.Contains(contactInfoSM.Code)) &&
                                                          (contactInfoSM.Info == null || c.Info.Contains(contactInfoSM.Info)) &&
                                                          (contactInfoSM.Description == null || c.Description.Contains(contactInfoSM.Description) || c.Description_Ar.Contains(contactInfoSM.Description)),
                                                          c => c.Id, contactInfoSM.PageNumber, contactInfoSM.PageSize, "success");
            if (result.Success)
                return new PagedResults<ContactInfoListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<ContactInfoListVM>>(result.Entities),
                };
            return new PagedResults<ContactInfoListVM> { Message = ErrorMessages.GeneralError };
        }

        //public Results<ContactInfoListVM> Get(ContactInfoSM contactInfoSM)
        //{
        //    var result = _contactInfoRepository.GetMany(c =>
        //                                                  (contactInfoSM.Code == null || c.Code.Contains(contactInfoSM.Code)) &&
        //                                                  (contactInfoSM.Info == null || c.Info.Contains(contactInfoSM.Info)) &&
        //                                                  (contactInfoSM.Description == null || c.Description.Contains(contactInfoSM.Description) || c.Description_Ar.Contains(contactInfoSM.Description)),
        //                                                  c => c.Id, "success");
        //    if (result.Success)
        //        return new Results<ContactInfoListVM>
        //        {
        //            Success = true,
        //            Entities = Mapper.Map<IList<ContactInfoListVM>>(result.Entities),
        //        };
        //    return new Results<ContactInfoListVM> { Message = ErrorMessages.GeneralError };
        //}

        public Result Update(IEnumerable<ContactInfoDetailsVM> categories)
        {
            var result = _contactInfoRepository.UpdateMany(Mapper.Map<IEnumerable<ContactInfo>>(categories), Messages.Updated);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
