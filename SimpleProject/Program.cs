using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SimpleProject.Data;
using SimpleProject.Resources;
using SimpleProject.Services.Implementations;
using SimpleProject.Services.Interfaces;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


//connction to database
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration["ConnectionStrings:dbcontext"]));


//create one instance for the same request .
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromMinutes(5);
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.Path = "/";
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = ".SimpleProject";

});

#region Localization
builder.Services.AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider=(type, factory) =>
                    factory.Create(typeof(SharedResources));
                });
builder.Services.AddLocalization(opt =>
    {
        opt.ResourcesPath = "";
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
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
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/home/error");
    app.UseHsts();
}

#region Localization Middleware

var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options!.Value);

#endregion
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//app.MapDefaultControllerRoute();


app.MapControllerRoute
    (
    name: "default",
    pattern: "{Controller=Home}/{Action=Index}/{Id?}"
    );

app.Run();
