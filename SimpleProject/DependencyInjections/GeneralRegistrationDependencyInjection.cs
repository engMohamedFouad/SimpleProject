using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using System.Reflection;

namespace SimpleProject.DependencyInjections
{
    public static class GeneralRegistrationDependencyInjection
    {
        public static IServiceCollection AddGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            //connction to database
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(configuration["ConnectionStrings:dbcontext"]));
            services.AddControllersWithViews();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
             {
                 options.IOTimeout = TimeSpan.FromMinutes(5);
                 options.IdleTimeout = TimeSpan.FromMinutes(5);
                 options.Cookie.Path = "/";
                 options.Cookie.IsEssential = true;
                 options.Cookie.HttpOnly = true;
                 options.Cookie.Name = ".SimpleProject";

             });


            return services;
        }
    }
}
