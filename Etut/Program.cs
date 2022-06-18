using Etut.Data;
using Etut.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Builder;
using Etut.Services;
using Etut.Models.DataModels;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    // Add Run-time compilation
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Add Application DB Context
    var dbConnectionString = builder.Configuration["ConnectionStrings:DevelopmentConnection"];
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(dbConnectionString);
    });
    // Add cusotm Identity 
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

    // Set the URL to lowercase
    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    // Add IUserService Service for Dependecy Injection
    builder.Services.AddTransient<IUserService, UserService>();
    // Add ICourseService Service for Dependecy Injection
    builder.Services.AddTransient<ICourseService, CourseService>();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseDeveloperExceptionPage();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();
    app.UseAuthentication();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
