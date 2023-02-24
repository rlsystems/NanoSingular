using NanoSingular.RazorApi.Extensions;
using NanoSingular.RazorApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationServices(builder.Configuration); // register services
var app = builder.Build(); // create the app


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

app.UseCors("defaultPolicy"); // CORS policy (default - allow any orgin)
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TenantResolver>();
app.MapRazorPages();
app.MapControllers();

app.SeedDatabase(); // run the DbInitializer (seed non-static data - root tenant/admin, roles)


app.Run();