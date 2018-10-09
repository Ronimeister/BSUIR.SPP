using Loggers;
using Loggers.Implementation;
using Ninject;
using Validation.Interfaces;
using Validation.Service;

namespace DIResolver
{
    public static class ResolverConfig
    {
        public static void ResolveDependencies(this IKernel kernel)
        {
            kernel.Bind<ILogger>().To<NLogLogger>();
            kernel.Bind(typeof(IValidationService<>)).To(typeof(ValidationService<>));
        }
    }
}
