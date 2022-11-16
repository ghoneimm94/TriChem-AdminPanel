using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.CustomerCertificate.SearchModels;
using TriChem.Models.CustomerCertificate.ViewModels;

namespace TriChem.API.Controllers
{
    public class CustomerCertificateController : ApiController
    {
        #region Services
        private readonly ICustomerCertificateService _customerCertificateService;
        #endregion

        #region Constructor
        public CustomerCertificateController(ICustomerCertificateService customerCertificateService)
        {
            _customerCertificateService = customerCertificateService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public PagedResults<CustomerCertificateListVM> Get([FromUri]CustomerCertificateSM customerCertificateSM)
        {
            var result = _customerCertificateService.Get(customerCertificateSM);
            if (!result.Success)
                return new PagedResults<CustomerCertificateListVM> { Message = result.Message };
            return new PagedResults<CustomerCertificateListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            };
        }

        // GET: api/CustomerCertificate/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CustomerCertificate
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CustomerCertificate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CustomerCertificate/5
        public void Delete(int id)
        {
        }
        #endregion
    }
}
