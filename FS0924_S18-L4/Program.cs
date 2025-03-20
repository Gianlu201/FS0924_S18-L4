using FS0924_S18_L4.Data;
using FS0924_S18_L4.Models;
using FS0924_S18_L4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder
    .Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireConfirmedAccount");

        options.Password.RequiredLength = builder
            .Configuration.GetSection("Identity")
            .GetValue<int>("RequiredLength");

        options.Password.RequireDigit = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireDigit");

        options.Password.RequireLowercase = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireLowercase");

        options.Password.RequireNonAlphanumeric = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireNonAlphanumeric");

        options.Password.RequireUppercase = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireUppercase");
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.Cookie.Name = "EpiSchoolAuth";
    });

builder.Services.AddScoped<SchoolClassService>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<ApplicationRole>>();

var app = builder.Build();

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

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
