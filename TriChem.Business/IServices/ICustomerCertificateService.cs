using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.CustomerCertificate.SearchModels;
using TriChem.Models.CustomerCertificate.ViewModels;

namespace TriChem.Business.IServices
{
    public interface ICustomerCertificateService
    {
        PagedResults<CustomerCertificateListVM> Get(CustomerCertificateSM CustomerCertificateSM);
        Results<CustomerCertificateListVM> Get();
        Result<CustomerCertificateDetailsVM> Get(int id);
        Result<CustomerCertificateDetailsVM> Add(CustomerCertificateDetailsVM CustomerCertificateVM);
        Result Update(IEnumerable<CustomerCertificateDetailsVM> CustomerCertificates);
        Result Delete(IEnumerable<int> ids);
    }
}
