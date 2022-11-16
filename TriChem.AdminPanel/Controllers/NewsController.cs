using System.Collections.Generic;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using TriChem.AdminPanel.Models;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.Helpers.Utilities;
using TriChem.Models.News.SearchModels;
using TriChem.Models.News.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class NewsController : Controller
    {
        #region Services
        private readonly INewsService _newsService;
        //private readonly IService<NewsListVM, NewsListWithChildsVM, NewsDetailsVM, NewsSM> _newsService;
        #endregion

        #region Constructor
        public NewsController()
        {
            _newsService = new NewsService();
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(NewsSM newsSM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _newsService.Get(newsSM);
            if (!result.Success)
                return View(new PagedResults<NewsListVM> { Message = result.Message });
            return View(new PagedResults<NewsListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _newsService.Get(id);
            if (!result.Success)
                return View(new Result<NewsDetailsVM> { Message = result.Message });
            return View(new Result<NewsDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _newsService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return View(result.Entity);
        }

        public ActionResult Delete(int id)
        {
            var result = _newsService.Delete(new List<int> { id });
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
        public ActionResult Create(NewsDetailsVM newsVM, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid || Image!=null)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    newsVM.ImageURL = FileManager.Upload(Image, "/img/News");
                    var result = _newsService.Add(newsVM);
                  
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
        public ActionResult Edit(NewsDetailsVM newsVM, HttpPostedFileBase Image)
        {
            var oldValue = _newsService.Get(newsVM.Id);

            if (ModelState.IsValid)
            {
                if(Image!=null)
                {
                    var oldPath = "~/img/News" + oldValue.Entity.ImageURL.Substring(oldValue.Entity.ImageURL.LastIndexOf('/'));
                    FileManager.Delete(oldPath);
                    newsVM.ImageURL = FileManager.Upload(Image, "/img/News");
                }
            
                var result = _newsService.Update(new List<NewsDetailsVM> { newsVM });
                if (!result.Success)
                {
                    ViewBag.Message = result.Message;
                    return View(oldValue.Entity);
                }
                return RedirectToAction("Details", new { id = newsVM.Id });
            }
            return View(oldValue.Entity);
        }
        #endregion
        #endregion
    }
}