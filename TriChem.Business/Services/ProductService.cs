using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.Product.SearchModels;
using TriChem.Models.Product.ViewModels;
using TriChem.Resources.Global;
using TriChem.Resources.Product;

namespace TriChem.Business.Services
{
    public class ProductService : IProductService
    {
        #region Services
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductImage> _productImageRepository;
        #endregion

        #region Constructor
        //public ProductService(IRepository<Product> productRepository)
        public ProductService()
        {
            _productRepository = new Repository<Product>(new TriChemEntities());
            _productImageRepository = new Repository<ProductImage>(new TriChemEntities());
        }
        #endregion

        #region Methods
        public Result<ProductDetailsVM> Add(ProductDetailsVM productVM)
        {
            var entity = Mapper.Map<Product>(productVM);
            foreach (var item in productVM.ImageURLs)
            {
                entity.ProductImage.Add(new ProductImage
                {
                    ImageURL = item
                });
            }
            var result = _productRepository.AddOne(entity, Messages.Added);
            if (result.Success)
                return new Result<ProductDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<ProductDetailsVM>(result.Entity) };
            return new Result<ProductDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var result = _productRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<ProductListVM> Get()
        {
            var result = _productRepository.GetMany(null, c => c.Id, "success");
            if (result.Success)
                return new Results<ProductListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<ProductListVM>>(result.Entities),
                };
            return new Results<ProductListVM> { Message = ErrorMessages.GeneralError };
        }

        public Results<ProductListVM> Get(string query, int PageNumber = 1)
        {
            var result = _productRepository.GetMany(c =>
                              c.Title.Contains(query) ||
                              c.Title_Ar.Contains(query) ||
                              c.Description.Contains(query) ||
                              c.Description_Ar.Contains(query) ||
                              c.Category.Title.Contains(query) ||
                              c.Category.Title_Ar.Contains(query) ||
                              c.Category.Description.Contains(query) ||
                              c.Category.Description_Ar.Contains(query) ||
                              c.ProductFeatures.Contains(query) ||
                              c.ProductFeatures_Ar.Contains(query) ||
                              c.Properties.Contains(query) ||
                              c.Properties_Ar.Contains(query) ||
                              c.SubTitle.Contains(query) ||
                              c.SubTitle_Ar.Contains(query) ||
                              c.TypicalApplication.Contains(query) ||
                              c.TypicalApplication_Ar.Contains(query), c => c.Id, "success");
            if (result.Success)
                return new Results<ProductListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<List<ProductListVM>>(result.Entities),
                };
            return new Results<ProductListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<ProductDetailsVM> Get(int id)
        {
            var result = _productRepository.GetOne(c => c.Id == id, "success", p => p.ProductImage, p => p.Category);
            if (!result.Success)
                return new Result<ProductDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<ProductDetailsVM> { Success = true, Entity = Mapper.Map<ProductDetailsVM>(result.Entity) };
        }

        public PagedResults<ProductListVM> Get(ProductSM productSM)
        {
            var result = _productRepository.GetMany(c => (productSM.Title == null || c.Title.Contains(productSM.Title)
                                                                                  || c.Title_Ar.Contains(productSM.Title)) &&
                                                         (productSM.CategoryTitle == null || c.Category.Title.Contains(productSM.CategoryTitle)
                                                                                          || c.Category.Title_Ar.Contains(productSM.CategoryTitle)) &&
                                                         (productSM.CategoryId == 0 || c.CategoryId == productSM.CategoryId) &&
                                                         (productSM.Description == null || c.Description.Contains(productSM.Description)
                                                                                        || c.Description_Ar.Contains(productSM.Description)),
                                                         c => c.Index, productSM.PageNumber, productSM.PageSize, "success", p => p.ProductImage);
            if (result.Success)
                return new PagedResults<ProductListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<ProductListVM>>(result.Entities),
                };
            return new PagedResults<ProductListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Update(ProductDetailsVM product)
        {
            var productImage = new List<ProductImage>();
            var result = _productRepository.UpdateMany(Mapper.Map<List<Product>>(new List<ProductDetailsVM> { product }), Messages.Updated);
            dynamic result2;
            if (product.ImageURLs != null)
            {
                foreach (var item in product.ImageURLs)
                {
                    productImage.Add(new ProductImage
                    {
                        ProductId = product.Id,
                        ImageURL = item
                    });
                }
                result2 = _productImageRepository.AddMany(productImage, "");

            }
            else
            {
                result2 = new Result()
                {
                    Success = true
                };
            }

            if (result.Success && result2.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<ProductListVM> GetRelated(int id)
        {
            //var result = _productRepository.GetMany(p => p.LinkId.Contains("," + id.ToString() + ","), c => c.Id, "success");
            var linkIds = _productRepository.Select(p => p.Id == id, p => p.LinkId, "success").Collection.FirstOrDefault();
            if (linkIds == null)
            {
                return new Results<ProductListVM> { Message = ErrorMessages.GeneralError };
            }
            var result = _productRepository.GetMany(p => linkIds.Contains(p.Id.ToString()), c => c.Id, "success", p => p.ProductImage);
            if (result.Success)
                return new Results<ProductListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<ProductListVM>>(result.Entities),
                };
            return new Results<ProductListVM> { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
