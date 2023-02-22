using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Persistence.Contexts;
using NanoSingular.Infrastructure.Persistence.Initializer;
using NanoSingular.RazorApi.Extensions;
using NanoSingular.RazorApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(connectionString));

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    // Password Requirements
    o.Password.RequiredLength = 6;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
}
).AddEntityFrameworkStores<ApplicationDbContext>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
});

services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication/Login"; // specify which page is the login page

    // there used to be a mechnism in .net core called AutomaticAuthentication which
    // when set to true will ensure that request is authenticated, otherwise will only
    // authenticate if you have Authorize tag on the action/controller method.

    // This property was removed earlier, and although the recommended way
    // of acheiving the same is to use DefaultAuthenticationScheme and/or DefaultChallengeScheme

    // However, the above option wasn't working so as a work around
    // setting the property below fixes the issue.

    // I have opted-in for an alternative solution
    options.Events = new CookieAuthenticationEvents();
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
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TenantResolver>();
app.MapRazorPages();
app.MapControllers();

app.SeedDatabase();


app.Run();