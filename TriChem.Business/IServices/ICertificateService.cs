using System.Collections.Generic;
using TriChem.Business.Models;
using TriChem.Models.Certificate.SearchModels;
using TriChem.Models.Certificate.ViewModels;

namespace TriChem.Business.IServices
{
    public interface ICertificateService
    {
        PagedResults<CertificateListVM> Get(CertificateSM certificateSM);
        Results<CertificateListVM> Get();
        Result<CertificateDetailsVM> Get(int id);
        Result<CertificateDetailsVM> Add(CertificateDetailsVM certificateVM);
        Result Update(IEnumerable<CertificateDetailsVM> certificates);
        Result Delete(IEnumerable<int> ids);
    }
}
