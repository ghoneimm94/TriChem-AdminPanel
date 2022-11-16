using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriChem.Business.IServices;
using TriChem.Business.Services;

namespace TriChem.AdminPanel.Controllers
{
    public class BaseController : Controller
    {
        #region Services
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        #endregion  

        #region Constructor
        public BaseController()
        {
            _categoryService = new CategoryService();
            _productService = new ProductService();
        }
        #endregion

        #region SelectLists
        public SelectList GetCategoryList()
        {
            return new SelectList(_categoryService.Get().Entities, "Id", "Title");
        }

        public SelectList GetProductList()
        {
            return new SelectList(_productService.Get().Entities, "Id", "Title");
        }
        #endregion

    }
}