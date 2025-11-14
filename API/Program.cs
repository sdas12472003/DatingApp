
using API;
using API.Data;
using API.Entities;
using API.Extensions;
using API.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
.WithOrigins("http://localhost:4200", "https://localhost:4200"));


app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();


app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "Fallback");


using var scope = app.Services.CreateScope();
var services=scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
    // await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Connections\"");
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();

// using API;
// using API.Data;
// using API.Entities;
// using API.Extensions;
// using API.SignalR;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;

// var builder = WebApplication.CreateBuilder(args);

// // Add services
// builder.Services.AddApplicationServices(builder.Configuration);
// builder.Services.AddIdentityServices(builder.Configuration);

// // TEMPORARY: use localhost CORS until Angular deploys
// builder.Services.AddCors(opt =>
// {
//     opt.AddPolicy("AllowAngular", policy =>
//     {
//         policy.AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials()
//               .WithOrigins(
//                    "http://localhost:4200",
//                    "https://localhost:4200"
//               );
//     });
// });

// var app = builder.Build();

// // Middleware
// app.UseMiddleware<ExceptionMiddleware>();

// // *** IMPORTANT: CORS must come before authentication ***
// app.UseCors("AllowAngular");

// app.UseAuthentication();
// app.UseAuthorization();

// app.UseDefaultFiles();
// app.UseStaticFiles();

// app.MapControllers();
// app.MapHub<PresenceHub>("hubs/presence");
// app.MapHub<MessageHub>("hubs/message");
// app.MapFallbackToController("Index", "Fallback");

// // DB Migrations
// using var scope = app.Services.CreateScope();
// var services = scope.ServiceProvider;

// try
// {
//     var context = services.GetRequiredService<DataContext>();
//     var userManager = services.GetRequiredService<UserManager<AppUser>>();
//     var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

//     await context.Database.MigrateAsync();
//     await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
//     await Seed.SeedUsers(userManager, roleManager);
// }
// catch (Exception ex)
// {
//     var logger = services.GetRequiredService<ILogger<Program>>();
//     logger.LogError(ex, "An error occurred during migration");
// }

// // Render PORT binding
// // var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
// // app.Urls.Add($"http://0.0.0.0:{port}");

// app.Run();
