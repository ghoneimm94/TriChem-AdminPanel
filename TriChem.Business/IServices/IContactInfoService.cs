using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.ContactInfo.SearchModels;
using TriChem.Models.ContactInfo.ViewModels;

namespace TriChem.Business.IServices
{
    public interface IContactInfoService
    {
        PagedResults<ContactInfoListVM> Get(ContactInfoSM contactInfoSM);
        //Results<ContactInfoListVM> Get(ContactInfoSM contactInfoSM);
        Results<ContactInfoListVM> Get();
        Result<ContactInfoDetailsVM> Get(int id);
        Result<ContactInfoDetailsVM> Add(ContactInfoDetailsVM contactInfoVM);
        Result Update(IEnumerable<ContactInfoDetailsVM> contactInfos);
        Result Delete(IEnumerable<int> ids);
    }
}