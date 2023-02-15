using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Persistence.Contexts;
using NanoSingular.RazorApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.SignIn.RequireConfirmedAccount = true; // Password Requirements
    o.Password.RequiredLength = 6;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
}
).AddEntityFrameworkStores<ApplicationDbContext>();

services.AddAuthentication();
services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication/Login"; // specify which page is the login page
});

services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder() // require auth on all pages/endpoints by default
        .RequireAuthenticatedUser()
        .Build();
});

services.AddRazorPages();

services.ConfigureApplicationServices(builder.Configuration); // Register Services

services.AddControllers(opt =>
{
    var policyBuilder = new AuthorizationPolicyBuilder();

    var policy = policyBuilder
                    .RequireAuthenticatedUser()
                    .Build();

    opt.Filters.Add(new AuthorizeFilter(policy)); // makes so that all the controllers require authorization by default
});

services.AddValidatorsFromAssemblyContaining<NanoSingular.Application.Utility.IRequestValidator>();
services.AddValidatorsFromAssemblyContaining<NanoSingular.Infrastructure.Utility.IRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.SeedDatabase(); // run the DbInitializer (seed non-static data - root tenant/admin, roles)

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<UserResolver>();

app.Use(async (context, next) =>
{
    await next(context);
});

app.MapRazorPages();
app.MapControllers();

app.Run();