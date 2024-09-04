using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockProj.Data.Identity;
using StockProj.Middleware;
using StockTrading.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StockContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<StockContext>()
    .AddUserStore<UserStore<ApplicationUser,ApplicationRole,StockContext,Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole,StockContext,Guid>>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddCookie(option => {
    option.LoginPath = "Account/Login";
    option.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddScoped<CategoryServices>();
builder.Services.AddScoped<ItemsServices>();

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddDebug();
    config.AddConsole();
});
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders; 
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/CommonError");
    app.UseCustomExceptionHandlerMiddleware();
    app.UseHsts();
}

app.UseHttpLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
