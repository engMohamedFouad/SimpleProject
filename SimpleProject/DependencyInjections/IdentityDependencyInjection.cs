using Microsoft.AspNetCore.Identity;
using SimpleProject.Data;
using SimpleProject.Models.Identity;

namespace SimpleProject.DependencyInjections
{
    public static class IdentityDependencyInjection
    {
        public static IServiceCollection AddIdentityDependencyInjection(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                //Password Settings
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredLength=6;

                //LockOut Settings
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts=5;
                opt.Lockout.AllowedForNewUsers=true;

                //user Settings
                opt.User.RequireUniqueEmail = true;

                //SignIn Settings
                opt.SignIn.RequireConfirmedEmail = false;


            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            return services;
        }
    }
}
