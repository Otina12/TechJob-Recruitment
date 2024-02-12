using JobFindingWebsite.Data;
using JobFindingWebsite.Helpers;
using JobFindingWebsite.Interfaces;
using JobFindingWebsite.Models;
using JobFindingWebsite.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JobFindingWebsite.Services;
using RunGroopWebApp.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<IFrameworkRepository, FrameworkRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IFavAndSavVacancyRepository, FavAndSavRepository>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddIdentity<Account, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie();
//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if(args.Length == 1 && args[0] == "seed")
{
    await Seed.SeedUsersAndRolesAsync(app);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
