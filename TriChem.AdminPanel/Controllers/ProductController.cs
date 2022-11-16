using ChinhDo.Transactions;
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
using TriChem.Domain.Models;
using TriChem.Helpers.Utilities;
using TriChem.Models.Product.SearchModels;
using TriChem.Models.Product.ViewModels;

namespace TriChem.AdminPanel.Controllers
{
    public class ProductController : BaseController
    {
        #region Services
        private readonly IProductService _productService;
        //private readonly IService<ProductListVM, ProductListWithChildsVM, ProductDetailsVM, ProductSM> _productService;
        #endregion

        #region Constructor
        public ProductController()
        {
            _productService = new ProductService();
        }
        #endregion

        #region Actions
        #region Get
        public ActionResult Index(ProductSM productSM)
        {
            if (TempData["Deleted"] != null)
            {
                ViewBag.Deleted = TempData["Deleted"].ToString();
                ViewBag.DeletedMessage = TempData["DeletedMessage"].ToString();
            }

            var result = _productService.Get(productSM);
            if (!result.Success)
                return View(new PagedResults<ProductListVM> { Message = result.Message });
            return View(new PagedResults<ProductListVM>
            {
                Success = true,
                Entities = result.Entities,
                MetaData = result.Entities.GetMetaData()
            });
        }

        public ActionResult Details(int id)
        {
            var result = _productService.Get(id);
            var relatedResult = _productService.GetRelated(id);
            //if (!result.Success && relatedResult.Success)
            //    return View(new Result<ProductDetailsVM> { Message = result.Message });
            ViewBag.Related = relatedResult.Entities;
            return View(new Result<ProductDetailsVM> { Success = true, Entity = result.Entity });
        }

        public ActionResult Create()
        {
            ViewBag.CategoryTitleList = GetCategoryList();
            ViewBag.ProductTitleList = GetProductList();
            return View();
        }

        public ActionResult Edit(int id)
        {
            var result = _productService.Get(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            ViewBag.CategoryTitleList = GetCategoryList();
            ViewBag.ProductTitleList = GetProductList();
            return View(result.Entity);
        }

        public ActionResult Delete(int id)
        {
            var result = _productService.Delete(new List<int> { id });
            if (result.Success)
                TempData.Add("Deleted", true);
            else
                TempData.Add("Deleted", false);
            TempData.Add("DeletedMessage", result.Message);
            return RedirectToAction("Index");
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

                    var image = _db.ProductImage.Where(c => c.ImageURL == url).FirstOrDefault();
                    if (image == null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return Json(new { Result = "Error" });
                    }

                    //Remove from database
                    _db.ProductImage.Remove(image);
                    _db.SaveChanges();

                    //Delete file from the file system
                    var path = "~/img/Product" + image.ImageURL.Substring(image.ImageURL.LastIndexOf('/'));
                    FileManager.Delete(path);

                    return Json(new { Result = "OK" });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message });
                }
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public ActionResult Create(ProductDetailsVM productVM,
            HttpPostedFileBase Certificate, HttpPostedFileBase DataSheet_Ar, HttpPostedFileBase DataSheet, HttpPostedFileBase[] Images)
        {
            if (ModelState.IsValid)
            {
                using (TransactionScope scope = new TransactionScope())
                {             
                    if (Images != null && Images.FirstOrDefault()!=null)
                    {
                        productVM.ImageURLs = new List<string>();
                        for (int i = 0; i < Images.Count(); i++)
                            productVM.ImageURLs.Add(FileManager.Upload(Images[i], "/img/Product"));
                    }

                    if (Certificate != null)
                    {
                        productVM.CertificatePath = FileManager.Upload(Certificate, "/file/Product/Certificate");

                    }
                    if (DataSheet != null)
                    {
                        productVM.DataSheetPath = FileManager.Upload(DataSheet, "/file/Product/DataSheet");
                    }
                    if (DataSheet_Ar != null)
                    {
                        productVM.DataSheetPath_Ar = FileManager.Upload(DataSheet_Ar, "/file/Product/DataSheet");
                    }
                    
                    var result = _productService.Add(productVM);

                    if (result.Success)
                    {
                        scope.Complete();
                        return RedirectToAction("Details", new { id = result.Entity.Id });
                    }

                    //If failed to add
                    foreach (var imagePath in productVM.ImageURLs)
                    {
                        var path = "~/img/Product" + imagePath.Substring(imagePath.LastIndexOf('/'));
                        FileManager.Delete(path);
                    }

                    if (Certificate != null)
                    {
                        FileManager.Delete("/file/Product/Certificate" + productVM.CertificatePath.Substring(productVM.CertificatePath.LastIndexOf('/')));
                    }

                    if (DataSheet != null)
                    {
                        FileManager.Delete("/file/Product/DataSheet" + productVM.DataSheetPath.Substring(productVM.DataSheetPath.LastIndexOf('/')));
                    }

                    if (DataSheet_Ar != null)
                    {
                        FileManager.Delete("/file/Product/DataSheet" + productVM.DataSheetPath_Ar.Substring(productVM.DataSheetPath_Ar.LastIndexOf('/')));
                    }
                }
            }

            ViewBag.Message = "Failed to add please try again";
            ViewBag.CategoryTitleList = GetCategoryList();
            ViewBag.ProductTitleList = GetProductList();

            return View();
        }

        [HttpPost]
        public ActionResult Edit(ProductDetailsVM productVM, HttpPostedFileBase Certificate, HttpPostedFileBase[] Images, HttpPostedFileBase DataSheet_Ar, HttpPostedFileBase DataSheet)
        {
            var oldProduct = _productService.Get(productVM.Id);
            if (oldProduct.Entity == null)
            {
                ViewBag.Message = "Product not found please try again";
                ViewBag.CategoryTitleList = GetCategoryList();
                ViewBag.ProductTitleList = GetProductList();
                return View();
            }
            if (Images != null && Images.FirstOrDefault() != null)
            {
                productVM.ImageURLs = new List<string>();
                for (int i = 0; i < Images.Count(); i++)
                {
                    productVM.ImageURLs.Add(FileManager.Upload(Images[i], "/img/Product"));
                }
            }

            if (Certificate!=null)
            {
             
                if (!string.IsNullOrEmpty(oldProduct.Entity.CertificatePath))
                {
                    string relativeURL = "~/file/Product/Certificate" + productVM.CertificatePath.Substring(productVM.CertificatePath.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                }
                productVM.CertificatePath = FileManager.Upload(Certificate, "/file/Product/Certificate");
            }

            if (DataSheet!=null)
            {
                if(!string.IsNullOrEmpty(oldProduct.Entity.DataSheetPath))
                {
                    string relativeURL = "~/file/Product/DataSheet" + productVM.DataSheetPath.Substring(productVM.DataSheetPath.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                }
                
                productVM.DataSheetPath = FileManager.Upload(DataSheet, "/file/Product/DataSheet");
            }

            if (DataSheet_Ar != null)
            {
                if (!string.IsNullOrEmpty(oldProduct.Entity.DataSheetPath_Ar))
                {
                    string relativeURL = "~/file/Product/DataSheet" + productVM.DataSheetPath_Ar.Substring(productVM.DataSheetPath_Ar.LastIndexOf('/'));
                    FileManager.Delete(relativeURL);
                }
                productVM.DataSheetPath_Ar = FileManager.Upload(DataSheet_Ar, "/file/Product/DataSheet");
            }
        
            
            var result = _productService.Update(productVM);

            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View();
            }
            return RedirectToAction("Details", new { id = productVM.Id });
        }
        #endregion
        #endregion
    }
}