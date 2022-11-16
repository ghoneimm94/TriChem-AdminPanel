using AutoMapper;
using PagedList;
using TriChem.Domain.Models;
using TriChem.Models.Category.ViewModels;
using TriChem.Models.Certificate.ViewModels;
using TriChem.Models.Client.ViewModels;
using TriChem.Models.ClientFeedback.ViewModels;
using TriChem.Models.ContactInfo.ViewModels;
using TriChem.Models.CustomerCertificate.ViewModels;
using TriChem.Models.News.ViewModels;
using TriChem.Models.Product.ViewModels;
using System.Linq;
using System.Collections.Generic;

namespace TriChem.Business.AutoMapper
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryListVM>()
                .ForMember(dest => dest.ImageURLs, opt => opt.MapFrom(src => src.CategoryImage.Select(i => i.ImageURL)));

            CreateMap<Category, CategoryDetailsVM>()
                .ForMember(dest => dest.ImageURLs, opt => opt.MapFrom(src => src.CategoryImage.Select(i => i.ImageURL)));


            CreateMap<CategoryDetailsVM, Category>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Certificate, opt => opt.Ignore());
  
            //CreateMap<Category, CategoryListVM>();
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? src.Title : src.Title_Ar))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Description : src.Description_Ar));
            //CreateMap<CategoryListVM, Category>();

            CreateMap<IPagedList<Category>, IPagedList<CategoryListVM>>().
                ConvertUsing<PagedListConverter<Category, CategoryListVM>>();
        }
    }

    internal class ContactInfoProfile : Profile
    {
        public ContactInfoProfile()
        {
            CreateMap<ContactInfo, ContactInfoListVM>();
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Description : src.Description_Ar));
            CreateMap<IPagedList<ContactInfo>, IPagedList<ContactInfoListVM>>().
               ConvertUsing<PagedListConverter<ContactInfo, ContactInfoListVM>>();
        }
    }

    internal class CustomerCertificateProfile : Profile
    {
        public CustomerCertificateProfile()
        {
            CreateMap<CustomerCertificate, CustomerCertificateListVM>();
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Title : src.Title_Ar));

            CreateMap<IPagedList<CustomerCertificate>, IPagedList<CustomerCertificateListVM>>().
                ConvertUsing<PagedListConverter<CustomerCertificate, CustomerCertificateListVM>>();
        }
    }

    internal class CertificateProfile : Profile
    {
        public CertificateProfile()
        {
            CreateMap<Certificate, CertificateListVM>()
                .ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title))
                .ForMember(dest => dest.CategoryTitle_Ar, opt => opt.MapFrom(src => src.Category.Title_Ar));

            CreateMap<Certificate, CertificateDetailsVM>()
               .ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title));
              

            CreateMap<CertificateDetailsVM, Certificate>().ForMember(dest => dest.Category, opt => opt.Ignore());
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Title : src.Title_Ar))
            //.ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Category.Title : src.Category.Title_Ar));

            CreateMap<IPagedList<Certificate>, IPagedList<CertificateListVM>>().
                ConvertUsing<PagedListConverter<Certificate, CertificateListVM>>();
        }
    }

    internal class NewsProfile : Profile
    {
        public NewsProfile()
        {
            CreateMap<News, NewsListVM>();
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Title : src.Title_Ar))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Description : src.Description_Ar));

            CreateMap<IPagedList<News>, IPagedList<NewsListVM>>().
                ConvertUsing<PagedListConverter<News, NewsListVM>>();

            CreateMap<News, NewsDetailsVM>();
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Title : src.Title_Ar))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Description : src.Description_Ar));
        }
    }

    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductListVM>().ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ProductImage.FirstOrDefault().ImageURL));
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Title : src.Title_Ar))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Description : src.Description_Ar));

            CreateMap<IPagedList<Product>, IPagedList<ProductListVM>>().
                ConvertUsing<PagedListConverter<Product, ProductListVM>>();

            CreateMap<Product, ProductDetailsVM>()
                .ForMember(dest => dest.ImageURLs, opt => opt.MapFrom(src => src.ProductImage.Select(i => i.ImageURL)))
                .ForMember(dest => dest.LinkId, opt => opt.MapFrom(src => src.LinkId.Split(',')))
                .ForMember(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title));

            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Title : src.Title_Ar))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Description : src.Description_Ar));
            CreateMap<ProductDetailsVM, Product>()
                //.ForMember(dest => dest.ProductImage., opt => opt.MapFrom(src => src.ImageURLs))
                .ForMember(dest => dest.LinkId, opt => opt.ResolveUsing(src =>
                {
                    string concatenatedIds = string.Empty;
                    src.LinkId.ToList().ForEach(id => concatenatedIds += id + ",");
                    concatenatedIds = concatenatedIds.TrimEnd(',');
                    return concatenatedIds;
                }));
        }
    }

    internal class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientListVM>();
            //.ForMember(dest => dest.Title, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Title : src.Title_Ar));

            CreateMap<IPagedList<Client>, IPagedList<ClientListVM>>().
                ConvertUsing<PagedListConverter<Client, ClientListVM>>();

            //CreateMap<Client, ClientDetailsVM>()
            //   .ForMember(dest => dest.Title, opt => opt.MapFrom(
            //              src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //              src.Title : src.Title_Ar));
        }
    }

    internal class ClientFeedbackProfile : Profile
    {
        public ClientFeedbackProfile()
        {
            CreateMap<ClientFeedback, ClientFeedbackListVM>();
            //.ForMember(dest => dest.ClientName, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.ClientName : src.ClientName_Ar))
            //.ForMember(dest => dest.ClientPosition, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.ClientPosition : src.ClientPosition_Ar))
            //.ForMember(dest => dest.Message, opt => opt.MapFrom(
            //           src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //           src.Message : src.Message_Ar));

            CreateMap<IPagedList<ClientFeedback>, IPagedList<ClientFeedbackListVM>>().
                ConvertUsing<PagedListConverter<ClientFeedback, ClientFeedbackListVM>>();

            //CreateMap<Client, ClientDetailsVM>()
            //   .ForMember(dest => dest.Title, opt => opt.MapFrom(
            //              src => Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
            //              src.Title : src.Title_Ar));
        }
    }
}