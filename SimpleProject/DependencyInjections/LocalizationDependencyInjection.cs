using Microsoft.AspNetCore.Localization;
using SimpleProject.Resources;
using System.Globalization;

namespace SimpleProject.DependencyInjections
{
    public static class LocalizationDependencyInjection
    {
        public static IServiceCollection AddLocalizationDependencyInjection(this IServiceCollection services)
        {
            #region Localization
            services.AddControllersWithViews()
                            .AddViewLocalization()
                            .AddDataAnnotationsLocalization(options =>
                            {
                                options.DataAnnotationLocalizerProvider=(type, factory) =>
                                factory.Create(typeof(SharedResources));
                            });
            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "";
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en-US"),
            new CultureInfo("ar-EG")
        };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion
            return services;
        }
    }
}
