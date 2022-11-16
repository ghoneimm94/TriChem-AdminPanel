using System.Collections.Generic;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TriChem.AdminPanel.Models;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.Helpers.Utilities;
using TriChem.Models.CustomerCertificate.SearchModels;
using TriChem.Models.CustomerCertificate.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class CustomerCertificateController : Controller
    {
        #region Services
        private readonly ICustomerCertificateService _customerCertificateService;
        //private readonly IService<CustomerCertificateListVM, CustomerCertificateListWithChildsVM, CustomerCertificateDetailsVM, CustomerCertificateSM> _customerCertificateService;
        #endregion

        #region Constructor
        public CustomerCertificateController()
        {
            _customerCertificateService = new CustomerCertificateService();
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(CustomerCertificateSM customerCertificateSM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _customerCertificateService.Get(customerCertificateSM);
            if (!result.Success)
                return View(new PagedResults<CustomerCertificateListVM> { Message = result.Message });
            return View(new PagedResults<CustomerCertificateListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _customerCertificateService.Get(id);
            if (!result.Success)
                return View(new Result<CustomerCertificateDetailsVM> { Message = result.Message });
            return View(new Result<CustomerCertificateDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _customerCertificateService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return View(result.Entity);
        }

        public ActionResult Delete(int id)
        {
            var result = _customerCertificateService.Delete(new List<int> { id });
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
        public ActionResult Create(CustomerCertificateDetailsVM customerCertificateVM, HttpPostedFileBase Image, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    // var result = _customerCertificateService.Add(customerCertificateVM);
                    if (Image != null && File != null)
                    {
                        customerCertificateVM.ImageURL = FileManager.Upload(Image, "/img/CustomerCertificate");
                        customerCertificateVM.FilePath = FileManager.Upload(File, "/file/CustomerCertificate");
                        var result = _customerCertificateService.Add(customerCertificateVM);
                        if (result.Success)
                        {
                            scope.Complete();
                            return RedirectToAction("Details", new { id = result.Entity.Id });
                        }
                        ViewBag.Message = result.Message;
                    }
                    ViewBag.Message = "image or file not uploaded";

                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(CustomerCertificateDetailsVM customerCertificateVM, HttpPostedFileBase Image, HttpPostedFileBase File)
        {
            var oldCertificate = _customerCertificateService.Get(customerCertificateVM.Id);
            if (ModelState.IsValid)
            {
                if (Image != null && File != null)
                {
                    var relativeURL = "~/img/CustomerCertificate" + customerCertificateVM.ImageURL.Substring(customerCertificateVM.ImageURL.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                    relativeURL = "~/file/CustomerCertificate" + customerCertificateVM.FilePath.Substring(customerCertificateVM.FilePath.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                    customerCertificateVM.ImageURL = FileManager.Upload(Image, "/img/CustomerCertificate");
                    customerCertificateVM.FilePath = FileManager.Upload(File, "/file/CustomerCertificate");
                }

              
                var result = _customerCertificateService.Update(new List<CustomerCertificateDetailsVM> { customerCertificateVM });

                if (!result.Success)
                {
                    ViewBag.Message = result.Message;
                    return View(oldCertificate.Entity);
                }
                return RedirectToAction("Details", new { id = customerCertificateVM.Id });
            }
            return View(oldCertificate.Entity);
        }
        #endregion
        #endregion
    }
}