using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.Certificate.SearchModels;
using TriChem.Models.Certificate.ViewModels;

namespace TriChem.API.Controllers
{
    public class CertificateController : ApiController
    {
        #region Services
        private readonly ICertificateService _certificateService;
        #endregion

        #region Constructor
        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public PagedResults<CertificateListVM> Get([FromUri]CertificateSM CertificateSM)
        {
            var result = _certificateService.Get(CertificateSM);
            if (!result.Success)
                return new PagedResults<CertificateListVM> { Message = result.Message };
            return new PagedResults<CertificateListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            };
        }

        // GET: api/Certificate/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Certificate
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Certificate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Certificate/5
        public void Delete(int id)
        {
        }
        #endregion
    }
}
