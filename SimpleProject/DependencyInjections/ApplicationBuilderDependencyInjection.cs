using Microsoft.Extensions.Options;

namespace SimpleProject.DependencyInjections
{
    public static class ApplicationBuilderDependencyInjection
    {
        public static IApplicationBuilder AddApplicationBuilderDependencyInjection(this IApplicationBuilder app, IServiceProvider service)
        {
            #region Localization Middleware

            var options = service.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options!.Value);
            return app;
            #endregion

        }
    }
}
