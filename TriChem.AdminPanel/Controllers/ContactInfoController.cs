using System.Collections.Generic;
using System.Web.Mvc;
using TriChem.AdminPanel.Models;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.Models.ContactInfo.SearchModels;
using TriChem.Models.ContactInfo.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class ContactInfoController : Controller
    {
        #region Services
        private readonly IContactInfoService _contactInfoService;
        //private readonly IService<ContactInfoListVM, ContactInfoListWithChildsVM, ContactInfoDetailsVM, ContactInfoSM> _contactInfoService;
        #endregion

        #region Constructor
        public ContactInfoController()
        {
            _contactInfoService = new ContactInfoService();
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(ContactInfoSM contactInfoSM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _contactInfoService.Get(contactInfoSM);
            if (!result.Success)
                return View(new PagedResults<ContactInfoListVM> { Message = result.Message });
            return View(new PagedResults<ContactInfoListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _contactInfoService.Get(id);
            if (!result.Success)
                return View(new Result<ContactInfoDetailsVM> { Message = result.Message });
            return View(new Result<ContactInfoDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _contactInfoService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return View(result.Entity);
        }

        public ActionResult Delete(int id)
        {
            var result = _contactInfoService.Delete(new List<int> { id });
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
        public ActionResult Create(ContactInfoDetailsVM contactInfoVM)
        {
            if (ModelState.IsValid)
            {
                var result = _contactInfoService.Add(contactInfoVM);
                if (result.Success)
                    return RedirectToAction("Details", new { id = result.Entity.Id });
                ViewBag.Message = result.Message;
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ContactInfoDetailsVM contactInfoVM)
        {
            var result = _contactInfoService.Update(new List<ContactInfoDetailsVM> { contactInfoVM });

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return RedirectToAction("Details", new { id = contactInfoVM.Id });
        }
        #endregion
        #endregion
    }
}