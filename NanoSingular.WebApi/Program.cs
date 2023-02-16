using NanoSingular.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationServices(builder.Configuration); // Register Services / CORS / Configure Identity Requirements / JWT Settings / Register DB Contexts / Image Handling, Mailer, Fluent Validation, Automapper
var app = builder.Build(); // Create the App

app.UseCors("defaultPolicy"); // CORS policy (default - allow any orgin)
app.UseHttpsRedirection();

app.UseRouting();
app.UseDefaultFiles();  // enables serving static files from wwwroot folder (react client - index.html)
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToController("Index", "Fallback"); // directs all traffic to index.html
});

app.Run();