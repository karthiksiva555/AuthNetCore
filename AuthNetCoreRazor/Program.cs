using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthNetCoreRazor.Data;
using AuthNetCoreRazor.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString)
            .UseLazyLoadingProxies());

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// builder.Services.AddAuthentication()
//     .AddFacebook(options =>
//     {
//         options.AppId = "";
//         options.AppSecret = "";
//     })
//     .AddGoogle(options =>
//     {
//         options.ClientId = "";
//         options.ClientSecret = "";
//     });

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    
    // lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); // lock user for 10 minutes
    options.Lockout.MaxFailedAccessAttempts = 5;
    
    // user settings
    options.User.RequireUniqueEmail = true;
    
    // sign in options => same as the option in AddDefaultIdentity
    options.SignIn.RequireConfirmedAccount = true;
});
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();