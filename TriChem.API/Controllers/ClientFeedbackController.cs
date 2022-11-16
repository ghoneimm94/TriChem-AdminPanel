using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.ClientFeedback.ViewModels;

namespace TriChem.API.Controllers
{
    public class ClientFeedbackController : ApiController
    {
        #region Services
        private readonly IClientFeedbackService _clientFeedbackService;
        #endregion

        #region Constructor
        public ClientFeedbackController(IClientFeedbackService clientFeedbackService)
        {
            _clientFeedbackService = clientFeedbackService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public Results<ClientFeedbackListVM> Get()
        {
            var result = _clientFeedbackService.Get();
            if (!result.Success)
                return new Results<ClientFeedbackListVM> { Message = result.Message };
            return new Results<ClientFeedbackListVM> { Success = true, Entities = result.Entities };
        }

        // GET: api/Client/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Client
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Client/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Client/5
        public void Delete(int id)
        {
        }
        #endregion
    }
}
