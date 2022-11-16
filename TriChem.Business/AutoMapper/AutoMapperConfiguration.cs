using AutoMapper;
using PagedList;
using System.Collections.Generic;

namespace TriChem.Business.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<ContactInfoProfile>();
                cfg.AddProfile<CustomerCertificateProfile>();
                cfg.AddProfile<CertificateProfile>();
                cfg.AddProfile<CustomerCertificateProfile>();
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<ClientProfile>();
                cfg.AddProfile<ClientFeedbackProfile>();
                cfg.AddProfile<NewsProfile>();
            });
        }

        public static void Reset()
        {
            Mapper.Reset();
        }
    }

    internal class PagedListConverter<TSource, TDestination> :
        ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>
    {
        public IPagedList<TDestination> Convert(IPagedList<TSource> source, IPagedList<TDestination> destination,
            ResolutionContext context)
        {
            var dest = new List<TDestination>();
            foreach (var item in source) dest.Add(Mapper.Map<TSource, TDestination>(item));
            return new StaticPagedList<TDestination>(dest, source.GetMetaData());
        }
    }
}