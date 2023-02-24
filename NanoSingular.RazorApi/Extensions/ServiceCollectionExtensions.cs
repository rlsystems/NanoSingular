using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Infrastructure.Auth;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Images;
using NanoSingular.Infrastructure.Mailer;
using NanoSingular.Infrastructure.Mapper;
using NanoSingular.Infrastructure.Persistence.Contexts;

namespace NanoSingular.RazorApi.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region [-- CORS --]
            services.AddCors(p => p.AddPolicy("defaultPolicy", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));
            #endregion


            #region [-- ADD CONTROLLERS AND SERVICES --]
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy)); // makes so that all the controllers require authorization by default
            });
            services.AddRazorPages();
            services.AddValidatorsFromAssemblyContaining<Application.Utility.IRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<Infrastructure.Utility.IRequestValidator>();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

            services.AddServices(); // dynamic services registration
            #endregion


            #region [-- REGISTERING DB CONTEXT SERVICE --]
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            #endregion


            #region [-- SETTING UP IDENTITY CONFIGURATIONS --]
            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                // Password Requirements - set to your liking
                o.SignIn.RequireConfirmedAccount = false;

                o.Password.RequiredLength = 6;
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;

            }
            ).AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
            #endregion


            #region [-- AUTHENTICATION/AUTHORIZATION SETTINGS --]
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "/Authentication/Unauthorized";

                options.Events = new CookieAuthenticationEvents(); // requred to enforce proper middleware process flow, otherwise dbContext is accessed before middlware
            });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder() // require auth on all pages/endpoints by default
                    .RequireAuthenticatedUser()
                    .Build();
            });
            #endregion


        }
    }
}
