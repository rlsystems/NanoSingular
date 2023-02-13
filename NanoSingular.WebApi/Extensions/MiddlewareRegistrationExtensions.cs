using NanoSingular.WebApi.Middleware;

namespace NanoSingular.WebApi.Extensions
{
    public static class MiddlewareRegistrationExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserResolver>();

            // -- Add new middleware here

            return app;
        }
    }
}
