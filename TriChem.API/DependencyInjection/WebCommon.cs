using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using System;
using System.Data.Entity;
using System.Web;
using TriChem.API.DependencyInjection;
using TriChem.Business.IServices;
using TriChem.Business.Services;
using TriChem.DataAccess.Repositories;
using TriChem.Domain.Models;
using TriChem.Models.Category.SearchModels;
using TriChem.Models.Category.ViewModels;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebCommon), "Stop")]

namespace TriChem.API.DependencyInjection
{
    public static class WebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static IKernel Kernel => Bootstrapper.Kernel;

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services and repository here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //Repository
            kernel.Bind<IRepository<Category>>().To<Repository<Category>>().InSingletonScope();
            kernel.Bind<IRepository<ContactInfo>>().To<Repository<ContactInfo>>().InSingletonScope();
            kernel.Bind<IRepository<CustomerCertificate>>().To<Repository<CustomerCertificate>>().InSingletonScope();
            kernel.Bind<IRepository<Certificate>>().To<Repository<Certificate>>().InSingletonScope();
            kernel.Bind<IRepository<News>>().To<Repository<News>>().InSingletonScope();
            kernel.Bind<IRepository<Product>>().To<Repository<Product>>().InSingletonScope();
            kernel.Bind<IRepository<Client>>().To<Repository<Client>>().InSingletonScope();
            kernel.Bind<IRepository<ClientFeedback>>().To<Repository<ClientFeedback>>().InSingletonScope();

            //DbContext
            //kernel.Bind<DbContext>().ToConstructor(_ => new TriChemEntities());
            kernel.Bind<DbContext>().To<TriChemEntities>().InSingletonScope();

            //Services
            kernel.Bind<ICategoryService>().To<CategoryService>().InSingletonScope();            
            //kernel.Bind<IService<CategoryListVM, CategoryListWithChildsVM, CategoryDetailsVM, CategorySM>>().To<CategoryService>().InSingletonScope();            
            kernel.Bind<IContactInfoService>().To<ContactInfoService>().InSingletonScope();
            kernel.Bind<ICustomerCertificateService>().To<CustomerCertificateService>().InSingletonScope();
            kernel.Bind<ICertificateService>().To<CertificateService>().InSingletonScope();
            kernel.Bind<INewsService>().To<NewsService>().InSingletonScope();
            kernel.Bind<IProductService>().To<ProductService>().InSingletonScope();
            kernel.Bind<IClientService>().To<ClientService>().InSingletonScope();
            kernel.Bind<IClientFeedbackService>().To<ClientFeedbackService>().InSingletonScope();
        }
    }
}