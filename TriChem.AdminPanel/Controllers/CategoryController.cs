using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TriChem.AdminPanel.Models;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Helpers.Utilities;
using TriChem.Models.Category.SearchModels;
using TriChem.Models.Category.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class CategoryController : Controller
    {
        #region Services
        private readonly ICategoryService _categoryService;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<CategoryImage> _categoryImageRepository;
        //private readonly IService<CategoryListVM, CategoryListWithChildsVM, CategoryDetailsVM, CategorySM> _categoryService;
        #endregion

        #region Constructor
        public CategoryController()
        {
            _categoryService = new CategoryService();
            _categoryRepository = new Repository<Category>(new TriChemEntities());
            _categoryImageRepository = new Repository<CategoryImage>(new TriChemEntities());
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(CategorySM categorySM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _categoryService.Get(categorySM);
            if (!result.Success)
                return View(new PagedResults<CategoryListVM> { Message = result.Message });
            return View(new PagedResults<CategoryListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _categoryService.Get(id);
            if (!result.Success)
                return View(new Result<CategoryDetailsVM> { Message = result.Message });
            return View(new Result<CategoryDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _categoryService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return View(result.Entity);
        }

        public ActionResult Delete(int id, string[] imageURLs)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var result = _categoryService.Delete(new List<int> { id });
                if (result.Success)
                {
                    foreach (var item in imageURLs)
                    {
                        TempData.Add("Deleted", true);
                        FileManager.Delete("~/images/categories" + item.Substring(item.LastIndexOf('/')));
                        scope.Complete();
                    }
                   
                }
                else
                    TempData.Add("Deleted", false);
                TempData.Add("DeletedMessage", result.Message);
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public ActionResult Create(CategoryDetailsVM categoryVM, HttpPostedFileBase[] Images)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (Images != null)
                    {
                        categoryVM.ImageURLs = new List<string>();

                        foreach (var item in Images)
                        {
                            categoryVM.ImageURLs.Add(FileManager.Upload(item, "/images/categories"));
                        }
                    }
                    var result = _categoryService.Add(categoryVM);
                    if (result.Success)
                    {
                        scope.Complete();
                        return RedirectToAction("Details", new { id = result.Entity.Id });
                    }
                    ViewBag.Message = result.Message;
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(CategoryDetailsVM categoryVM, HttpPostedFileBase[] Images)
        {
            categoryVM.ImageURLs = new List<string>();
            if (Images != null && Images[0]!=null && Images.Length != 0)
            {           
                foreach (var item in Images)
                {
                   categoryVM.ImageURLs.Add(FileManager.Upload(item, "/images/categories"));
                }
              
            }

            var result = _categoryService.Update(categoryVM);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return RedirectToAction("Details", new { id = categoryVM.Id });
        }

        [HttpPost]
        public JsonResult DeleteFile(string url)
        {
            using (var _db = new TriChemEntities())
            {
                if (string.IsNullOrEmpty(url))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Result = "Error" });
                }
                try
                {

                    var image = _db.CategoryImage.Where(c => c.ImageURL == url).FirstOrDefault();
                    if (image == null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return Json(new { Result = "Error" });
                    }

                    //Remove from database
                    _db.CategoryImage.Remove(image);
                    _db.SaveChanges();

                    //Delete file from the file system
                    var relativeURL = "~/images/categories" + image.ImageURL.Substring(image.ImageURL.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                    return Json(new { Result = "OK" });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message });
                }
            }
        }
        #endregion
        #endregion
    }
}