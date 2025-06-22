
// using API;
// using API.Data;
// using API.Entities;
// using API.Extensions;
// using API.SignalR;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddApplicationServices(builder.Configuration);
// builder.Services.AddIdentityServices(builder.Configuration);
// var app = builder.Build();

// // Configure the HTTP request pipeline.
// app.UseMiddleware<ExceptionMiddleware>();
// app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
// .WithOrigins("http://localhost:4200", "https://localhost:4200"));


// app.UseAuthentication();
// app.UseAuthorization();

// app.UseDefaultFiles();
// app.UseStaticFiles();


// app.MapControllers();
// app.MapHub<PresenceHub>("hubs/presence");
// app.MapHub<MessageHub>("hubs/message");
// app.MapFallbackToController("Index", "Fallback");


// using var scope = app.Services.CreateScope();
// var services=scope.ServiceProvider;
// try
// {
//     var context=services.GetRequiredService<DataContext>();
//     var userManager = services.GetRequiredService<UserManager<AppUser>>();
//     var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
//     await context.Database.MigrateAsync();
//     await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
//     // await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Connections\"");
//     await Seed.SeedUsers(userManager, roleManager);
// }
// catch (Exception ex)
// {
//     var logger =services.GetRequiredService<ILogger<Program>>();
//     logger.LogError(ex,"An error occurred during migration");
// }
// app.Run();
using API;
using API.Data;
using API.Entities;
using API.Extensions;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Global exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

// CORS: allow local and deployed frontends
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(
        "http://localhost:4200",
        "https://localhost:4200",
        "https://datingapp-2i22.onrender.com"
    )
);

// Auth middleware
app.UseAuthentication();
app.UseAuthorization();

// Serve Angular static files
app.UseDefaultFiles();
app.UseStaticFiles();

// API controllers and SignalR hubs
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

// Fallback to index.html for Angular routes
app.MapFallbackToController("Index", "Fallback");

// Migration + seeding logic
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

    await context.Database.MigrateAsync();

    // Clear active connections table (startup cleanup)
    var sql = context.Database.ProviderName!.Contains("Sqlite")
        ? "DELETE FROM Connections"
        : "DELETE FROM [Connections]";
    await context.Database.ExecuteSqlRawAsync(sql);

    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
