using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.ContactInfo.SearchModels;
using TriChem.Models.ContactInfo.ViewModels;

namespace TriChem.API.Controllers
{
    public class ContactInfoController : ApiController
    {
        #region Services
        private readonly IContactInfoService _contactInfoService;
        #endregion

        #region Constructor
        public ContactInfoController(IContactInfoService contactInfoService)
        {
            _contactInfoService = contactInfoService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public Results<ContactInfoListVM> Get()
        {
            var result = _contactInfoService.Get();
            if (!result.Success)
                return new Results<ContactInfoListVM> { Message = result.Message };
            return new Results<ContactInfoListVM>
            {
                Success = true,
                Entities = result.Entities
            };
        }

        // GET: api/ContactInfo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ContactInfo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ContactInfo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ContactInfo/5
        public void Delete(int id)
        {
        }
        #endregion
    }
}
