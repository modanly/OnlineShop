using OnlineShop;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using OnlineShop.Db;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using OnlineShop.Helper;

var builder = WebApplication.CreateBuilder(args);
// получаем строку подключения из файла конфигурации
string connection = builder.Configuration.GetConnectionString("online_shop");


// добавляем контекст DatabaseContext в качестве сервиса в приложение
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

builder.Services.AddDbContext<IdentityContext>(options=> options.UseSqlServer(connection));

builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.Cookie = new CookieBuilder
    {
        IsEssential = true
    };
});

builder.Services.AddTransient<IProductsRepository, ProductsDbRepository>();
builder.Services.AddTransient<ICartsRepository, CartsDbRepository>();
builder.Services.AddTransient<IOrdersRepository, OrdersDbRepository>();
builder.Services.AddScoped<ImagesProvider>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<ProductsCache>();
//builder.Services.AddSingleton<IUsersManager, UsersManager>();
//builder.Services.AddSingleton<IRolesRepository, RolesInMemoryRepository>();

builder.Host.UseSerilog((context, configuration) => configuration
.ReadFrom.Configuration(context.Configuration)
.Enrich.WithProperty("ApplicationName", "Online Shop"));


// Add services to the container. 
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    IdentityInitializer.Initialize(userManager, rolesManager);
}

app.Run();
