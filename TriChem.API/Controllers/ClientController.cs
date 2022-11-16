using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TriChem.API.Models;
using TriChem.Business.IServices;
using TriChem.Models.Client.ViewModels;

namespace TriChem.API.Controllers
{
    public class ClientController : ApiController
    {
        #region Services
        private readonly IClientService _clientService;
        #endregion

        #region Constructor
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }
        #endregion

        #region Actions     
        [HttpGet]
        public Results<ClientListVM> Get()
        {
            var result = _clientService.Get();
            if (!result.Success)
                return new Results<ClientListVM> { Message = result.Message };
            return new Results<ClientListVM> { Success = true, Entities = result.Entities };
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
