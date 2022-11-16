using Ninject;
using System.Web.Http.Dependencies;

namespace TriChem.API.DependencyInjection
{
    public class Resolver : Scope, IDependencyResolver
    {
        private readonly IKernel _kernel;

        public Resolver(IKernel kernel)
            : base(kernel)
        {
            this._kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new Scope(_kernel);
        }
    }
}