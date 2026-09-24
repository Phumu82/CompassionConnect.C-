using CompassionConnect.Web.Data;
using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories;
using CompassionConnect.Web.Repositories.Interfaces;
using CompassionConnect.Web.Services;
using CompassionConnect.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Data / EF Core --------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

// --- ASP.NET Identity --------------------------------------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/LoginDonor";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
});

// --- Repositories / Unit of Work ---------------------------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --- Application services -----------------------------------------------------
builder.Services.AddScoped<IReferenceNumberService, ReferenceNumberService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaxCertificateService, TaxCertificateService>();
builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- Middleware pipeline -------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- Seed database (roles, demo accounts, sample data) --------------------------
using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

app.Run();
