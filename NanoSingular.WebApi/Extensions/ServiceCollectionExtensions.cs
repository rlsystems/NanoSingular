using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using FluentValidation.AspNetCore;
using System.Text;

using NanoSingular.Infrastructure.Auth;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Persistence.Contexts;
using NanoSingular.Infrastructure.Mapper;
using NanoSingular.Infrastructure.Mailer;
using NanoSingular.Infrastructure.Images;
using NanoSingular.Application.Utility;


// Configure Application Services
namespace NanoSingular.WebApi.Extensions
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

            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy)); // makes so that all the controllers require authorization by default

            }).AddFluentValidation(fv =>
            {
                fv.ImplicitlyValidateChildProperties = true;
                fv.ImplicitlyValidateRootCollectionElements = true;

                fv.RegisterValidatorsFromAssemblyContaining<IRequestValidator>(); // auto registers all fluent validation classes, in all assemblies with an IRequestValidator class
                fv.RegisterValidatorsFromAssemblyContaining<Infrastructure.Utility.IRequestValidator>();
            });

            services.AddEndpointsApiExplorer();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));

            services.AddServices(); // dynamic services registration

            //----------- Add Services (Dependency Injection) -------------------------------------------

            // From DynamicServiceRegistrationExtensions
            // Auto registers scoped/transient marked services 

            // ICurrentTenantUserService -- registered as Scoped (resolve the tenant/user from token/header)
            // IIdentityService, ITokenService, IRepositoryAsync, ITenantManagementService -- registered as Transient

            // Any additional app services should be registered as Transient

            //---------------------------------------------------------------------------

            #endregion

            #region [-- REGISTERING DB CONTEXT SERVICE --]

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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

            #region [-- JWT SETTINGS --]

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings")); // get settings from appsettings.json
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = configuration["JWTSettings:Issuer"],
                       ValidAudience = configuration["JWTSettings:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                   };
                   o.Events = new JwtBearerEvents()
                   {
                       OnChallenge = context =>
                       {        
                           context.HandleResponse(); 

                           context.Response.ContentType = "application/json";
                           context.Response.StatusCode = 401;

                           return context.Response.WriteAsync("Not Authorized");
                       },
                       OnForbidden = context =>
                       {
                           context.Response.StatusCode = 403;
                           context.Response.ContentType = "application/json";

                           return context.Response.WriteAsync("Not Authorized");
                       },
                   };
               });

            #endregion

        }
    }
}
