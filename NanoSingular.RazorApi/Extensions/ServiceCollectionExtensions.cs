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
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddServices(); // dynamic services registration


            #endregion

          

        }
    }
}
