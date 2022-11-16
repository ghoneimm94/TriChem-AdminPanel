using System.Collections.Generic;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TriChem.AdminPanel.Models;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.Helpers.Utilities;
using TriChem.Models.ClientFeedback.SearchModels;
using TriChem.Models.ClientFeedback.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class ClientFeedbackController : Controller
    {
        #region Services
        private readonly IClientFeedbackService _clientFeedbackService;
        //private readonly IService<ClientFeedbackListVM, ClientFeedbackListWithChildsVM, ClientFeedbackDetailsVM, ClientFeedbackSM> _clientFeedbackService;
        #endregion

        #region Constructor
        public ClientFeedbackController()
        {
            _clientFeedbackService = new ClientFeedbackService();
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(ClientFeedbackSM clientFeedbackSM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _clientFeedbackService.Get(clientFeedbackSM);
            if (!result.Success)
                return View(new PagedResults<ClientFeedbackListVM> { Message = result.Message });
            return View(new PagedResults<ClientFeedbackListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _clientFeedbackService.Get(id);
            if (!result.Success)
                return View(new Result<ClientFeedbackDetailsVM> { Message = result.Message });
            return View(new Result<ClientFeedbackDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _clientFeedbackService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return View(result.Entity);
        }

        public ActionResult Delete(int id)
        {
            var result = _clientFeedbackService.Delete(new List<int> { id });
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
        public ActionResult Create(ClientFeedbackDetailsVM clientFeedbackVM, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (Image != null)
                    {
                        clientFeedbackVM.ImageURL = FileManager.Upload(Image, "/img/ClientFeedback");
                        var result = _clientFeedbackService.Add(clientFeedbackVM);
                        if (result.Success)
                        {
                            scope.Complete();
                            return RedirectToAction("Details", new { id = result.Entity.Id });
                        }
                        ViewBag.Message = result.Message;
                    }
                    ViewBag.Message = "No image uploaded";
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ClientFeedbackDetailsVM clientFeedbackVM, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                var relativeURL = "~/img/ClientFeedback" + clientFeedbackVM.ImageURL.Substring(clientFeedbackVM.ImageURL.LastIndexOf('/'));
                FileManager.Delete(relativeURL);
                clientFeedbackVM.ImageURL = FileManager.Upload(Image, "/img/ClientFeedback");
            }

            var result = _clientFeedbackService.Update(new List<ClientFeedbackDetailsVM> { clientFeedbackVM });

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return RedirectToAction("Details", new { id = clientFeedbackVM.Id });
        }
        #endregion
        #endregion
    }
}