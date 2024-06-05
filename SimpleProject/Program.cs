using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Services.Implementations;
using SimpleProject.Services.Interfaces;
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
