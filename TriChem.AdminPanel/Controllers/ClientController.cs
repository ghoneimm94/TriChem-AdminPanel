using System.Collections.Generic;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TriChem.AdminPanel.Models;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.Helpers.Utilities;
using TriChem.Models.Client.SearchModels;
using TriChem.Models.Client.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class ClientController : Controller
    {
        #region Services
        private readonly IClientService _clientService;
        //private readonly IService<ClientListVM, ClientListWithChildsVM, ClientDetailsVM, ClientSM> _clientService;
        #endregion

        #region Constructor
        public ClientController()
        {
            _clientService = new ClientService();
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(ClientSM clientSM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _clientService.Get(clientSM);
            if (!result.Success)
                return View(new PagedResults<ClientListVM> { Message = result.Message });
            return View(new PagedResults<ClientListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _clientService.Get(id);
            if (!result.Success)
                return View(new Result<ClientDetailsVM> { Message = result.Message });
            return View(new Result<ClientDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _clientService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return View(result.Entity);
        }

        public ActionResult Delete(int id)
        {
            var result = _clientService.Delete(new List<int> { id });
            if (result.Success)
                TempData.Add("Deleted", true);
            else
                TempData.Add("Deleted", false);
            TempData.Add("DeletedMessage", result.Message);
            return RedirectToAction("Index");
        }
        #endregion

        #region Post
        [HttpPost]
        public ActionResult Create(ClientDetailsVM clientVM, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    // var result = _clientService.Add(clientVM);
                    if (Image != null)
                    {
                        clientVM.ImageURL = FileManager.Upload(Image, "/img/Client");
                        var result = _clientService.Add(clientVM);
                        if (result.Success)
                        {
                            scope.Complete();
                            return RedirectToAction("Details", new { id = result.Entity.Id });
                        }
                        ViewBag.Message = result.Message;
                    }
                    ViewBag.Message = "no image uploaded";
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ClientDetailsVM clientVM, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                var relativeURL = "~/img/Client" + clientVM.ImageURL.Substring(clientVM.ImageURL.LastIndexOf('/'));
                FileManager.Delete(relativeURL);
                clientVM.ImageURL = FileManager.Upload(Image, "/img/Client");
            }

            var result = _clientService.Update(new List<ClientDetailsVM> { clientVM });

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return RedirectToAction("Details", new { id = clientVM.Id });
        }
        #endregion
        #endregion
    }
}