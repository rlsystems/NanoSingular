using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Application.Utility;
using NanoSingular.Infrastructure.Auth;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Persistence.Contexts;
using NanoSingular.RazorApi.Extensions;
using NanoSingular.RazorApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.SignIn.RequireConfirmedAccount = false; // Password Requirements
    o.Password.RequiredLength = 6;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
}
).AddEntityFrameworkStores<ApplicationDbContext>()
 .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Authentication/Login"; // specify which page is the login page
});
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder() // require auth on all pages/endpoints by default
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationServices(builder.Configuration); // Register Services


builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy)); // makes so that all the controllers require authorization by default

}).AddFluentValidation(fv =>
{
    fv.ImplicitlyValidateChildProperties = true;
    fv.ImplicitlyValidateRootCollectionElements = true;

    fv.RegisterValidatorsFromAssemblyContaining<NanoSingular.Application.Utility.IRequestValidator>(); // auto registers all fluent validation classes, in all assemblies with an IRequestValidator class
    fv.RegisterValidatorsFromAssemblyContaining<NanoSingular.Infrastructure.Utility.IRequestValidator>();
});



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
app.UseMiddleware<UserResolver>();
app.MapRazorPages();
app.MapControllers();
app.SeedDatabase(); // run the DbInitializer (seed non-static data - root tenant/admin, roles)

app.Run();
