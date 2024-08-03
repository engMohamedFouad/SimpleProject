using SimpleProject.Repositories.Implementations;
using SimpleProject.Repositories.Interfaces;
using SimpleProject.SharedRepositories;

namespace SimpleProject.DependencyInjections
{
    public static class RepositoryDependencyhInjection
    {
        public static IServiceCollection AddRepositoryDependencyInjection(this IServiceCollection services)
        {

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IProductImagesRepository, ProductImagesRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
