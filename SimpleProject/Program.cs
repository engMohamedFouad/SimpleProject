using SimpleProject.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);


#region Register DependencyInjection
builder.Services.AddServiceDependencyInjection()
                .AddRepositoryDependencyInjection()
                .AddLocalizationDependencyInjection()
                .AddGeneralDependencyInjection(builder.Configuration);
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

app.AddApplicationBuilderDependencyInjection(app.Services);

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
