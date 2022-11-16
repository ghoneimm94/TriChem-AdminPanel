using System.Collections.Generic;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TriChem.AdminPanel.Models;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.Helpers.Utilities;
using TriChem.Models.Certificate.SearchModels;
using TriChem.Models.Certificate.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class CertificateController : BaseController
    {
        #region Services
        private readonly ICertificateService _certificateService;
        //private readonly IService<CertificateListVM, CertificateListWithChildsVM, CertificateDetailsVM, CertificateSM> _certificateService;
        #endregion

        #region Constructor
        public CertificateController()
        {
            _certificateService = new CertificateService();
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(CertificateSM certificateSM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _certificateService.Get(certificateSM);
            if (!result.Success)
                return View(new PagedResults<CertificateListVM> { Message = result.Message });
            return View(new PagedResults<CertificateListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _certificateService.Get(id);
            if (!result.Success)
                return View(new Result<CertificateDetailsVM> { Message = result.Message });
            return View(new Result<CertificateDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            ViewBag.CategoryTitleList = GetCategoryList();
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _certificateService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            ViewBag.CategoryTitleList = GetCategoryList();
            return View(result.Entity);
        }

        public ActionResult Delete(int id)
        {
            var result = _certificateService.Delete(new List<int> { id });
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
        public ActionResult Create(CertificateDetailsVM certificateVM,
            HttpPostedFileBase Image, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //var result = _certificateService.Add(certificateVM);
                    if (Image != null && File != null)
                    {
                        certificateVM.ImageURL = FileManager.Upload(Image, "/img/Certificate");
                        certificateVM.FilePath = FileManager.Upload(File, "/file/Certificate");
                        var result = _certificateService.Add(certificateVM);
                        if (result.Success)
                        {
                            scope.Complete();
                            return RedirectToAction("Details", new { id = result.Entity.Id });
                        }
                        ViewBag.Message = result.Message;
                    }
                    ViewBag.Message = "please select file or imge";
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(CertificateDetailsVM certificateVM, HttpPostedFileBase Image, HttpPostedFileBase File)
        {
            if (Image != null || File != null)
            {
                var certificate = _certificateService.Get(certificateVM.Id);
                var relativeURL = "";
                if (Image != null && certificate.Entity.ImageURL != null)
                {
                    relativeURL = "~/img/Certificate" + certificate.Entity.ImageURL.Substring(certificate.Entity.ImageURL.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                }
                certificateVM.FilePath = FileManager.Upload(File, "/file/Certificate");

                if (File != null && certificate.Entity.FilePath != null)
                {
                    relativeURL = "~/img/Certificate" + certificate.Entity.FilePath.Substring(certificate.Entity.FilePath.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                }
                certificateVM.ImageURL = FileManager.Upload(Image, "/img/Certificate");

            }

            var result = _certificateService.Update(new List<CertificateDetailsVM> { certificateVM });

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return RedirectToAction("Details", new { id = certificateVM.Id });
        }
        #endregion
        #endregion
    }
}