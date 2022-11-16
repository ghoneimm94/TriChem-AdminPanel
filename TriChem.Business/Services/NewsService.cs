using System;
using System.Collections.Generic;
using AutoMapper;
using PagedList;
using TriChem.Business.IServices;
using TriChem.Business.Models;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.News.SearchModels;
using TriChem.Models.News.ViewModels;
using TriChem.Resources.Global;
using TriChem.Resources.News;
using System.Linq;

namespace TriChem.Business.Services
{
    public class NewsService : INewsService
    {
        #region Services
        private readonly IRepository<News> _newsRepository;
        #endregion

        #region Constructor
        //public NewsService(IRepository<News> newsRepository)
        public NewsService()
        {
            _newsRepository = new Repository<News>(new TriChemEntities());
        }
        #endregion

        #region Methods
        public Result<NewsDetailsVM> Add(NewsDetailsVM newsVM)
        {
            var result = _newsRepository.AddOne(Mapper.Map<News>(newsVM), Messages.Added);
         
            if (result.Success)
                return new Result<NewsDetailsVM> { Success = true, Message = result.Message, Entity = Mapper.Map<NewsDetailsVM>(result.Entity) };
            return new Result<NewsDetailsVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Delete(IEnumerable<int> ids)
        {
            var result = _newsRepository.DeleteMany(c => ids.Contains(c.Id), Messages.Deleted);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }

        public Results<NewsListVM> Get()
        {
            var result = _newsRepository.GetMany(null, c => c.Id, "success");
            if (result.Success)
                return new Results<NewsListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IList<NewsListVM>>(result.Entities),
                };
            return new Results<NewsListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result<NewsDetailsVM> Get(int id)
        {
            var result = _newsRepository.GetOne(c => c.Id == id, "success");
            if (!result.Success)
                return new Result<NewsDetailsVM> { Message = ErrorMessages.GeneralError };
            return new Result<NewsDetailsVM> { Success = true, Entity = Mapper.Map<NewsDetailsVM>(result.Entity) };
        }

        public PagedResults<NewsListVM> Get(NewsSM newsSM)
        {
            var result = _newsRepository.GetMany(c =>
                                                          (newsSM.Title == null || c.Title.Contains(newsSM.Title) || c.Title_Ar.Contains(newsSM.Title)) &&
                                                          (newsSM.Description == null || c.Description.Contains(newsSM.Description) || c.Description_Ar.Contains(newsSM.Description)) &&
                                                          (newsSM.LinkId == null || c.LinkId.Contains(newsSM.LinkId)),
                                                          c => c.Id, newsSM.PageNumber, newsSM.PageSize, "success");
            if (result.Success)
                return new PagedResults<NewsListVM>
                {
                    Success = true,
                    Entities = Mapper.Map<IPagedList<NewsListVM>>(result.Entities),
                };
            return new PagedResults<NewsListVM> { Message = ErrorMessages.GeneralError };
        }

        public Result Update(IEnumerable<NewsDetailsVM> categories)
        {
            var result = _newsRepository.UpdateMany(Mapper.Map<IEnumerable<News>>(categories), Messages.Updated);
            if (result.Success)
                return new Result { Success = true, Message = result.Message };
            return new Result { Message = ErrorMessages.GeneralError };
        }
        #endregion
    }
}
