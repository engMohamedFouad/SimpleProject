using SimpleProject.Services.Implementations;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.DependencyInjections
{
    public static class ServiceDependencyhInjection
    {
        public static IServiceCollection AddServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
