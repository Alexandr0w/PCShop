using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using static PCShop.GCommon.ExceptionMessages;

namespace PCShop.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly string ProjectInterfacePrefix = "I";
        private static readonly string ServiceTypeSuffix = "Service";

        private static readonly string RepositoryTypeSuffix = "Repository";

        public static IServiceCollection AddUserDefinedServices(this IServiceCollection serviceCollection, Assembly serviceAssembly)
        {
            Type[] serviceClasses = serviceAssembly
                .GetTypes()
                .Where(t => !t.IsInterface && t.Name.EndsWith(ServiceTypeSuffix))
                .ToArray();

            foreach (Type serviceClass in serviceClasses)
            {
                Type[] serviceClassInterfaces = serviceClass
                    .GetInterfaces();

                if (serviceClassInterfaces.Length == 1 &&
                    serviceClassInterfaces.First().Name.StartsWith(ProjectInterfacePrefix) &&
                    serviceClassInterfaces.First().Name.EndsWith(ServiceTypeSuffix))
                {
                    Type serviceClassInterface = serviceClassInterfaces.First();

                    serviceCollection.AddScoped(serviceClassInterface, serviceClass);
                }
            }

            return serviceCollection;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection,
            Assembly repositoryAssembly)
        {
            Type[] repositoryClasses = repositoryAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith(RepositoryTypeSuffix) &&
                            !t.IsInterface &&
                            !t.IsAbstract)
                .ToArray();
            foreach (Type repositoryClass in repositoryClasses)
            {
                Type? repositoryInterface = repositoryClass
                    .GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"{ProjectInterfacePrefix}{repositoryClass.Name}");
                if (repositoryInterface == null)
                {
                    // Better solution, because it will throw an exception during application start-up
                    throw new ArgumentException(string.Format(InterfaceNotFoundMessage, repositoryClass.Name));
                }

                serviceCollection.AddScoped(repositoryInterface, repositoryClass);
            }

            return serviceCollection;
        }
    }
}