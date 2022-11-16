using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TriChem.AdminPanel.DependencyInjection
{
    public class Resolver : IDependencyResolver
    {
        protected IResolutionRoot ResolutionRoot;

        public Resolver(IKernel kernel)
        {
            ResolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = ResolutionRoot.CreateRequest(serviceType, null,
               new Parameter[0], true, true);
            return ResolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = ResolutionRoot.CreateRequest(serviceType, null,
               new Parameter[0], true, true);
            return ResolutionRoot.Resolve(request).ToList();
        }
    }
}