using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    // public static IServiceCollection AddIdentityServices(this IServiceCollection services,
    // IConfiguration config)
    // {
    //     services.AddIdentityCore<AppUser>(opt =>
    //     {
    //         opt.Password.RequireNonAlphanumeric = false;
    //     })
    //         .AddRoles<AppRole>()
    //         .AddRoleManager<RoleManager<AppRole>>()
    //         .AddEntityFrameworkStores<DataContext>();


    //     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //     .AddJwtBearer(options=>
    //     {
    //         // var tokenKey = config["TokenKey"] ?? throw new Exception("Token not found");
    //         // options.TokenValidationParameters = new TokenValidationParameters
    //         // {
    //         //     ValidateIssuerSigningKey = true,
    //         //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
    //         //     ValidateIssuer = false,
    //         //     ValidateAudience = false
    //         // };
    //         // options.Events = new JwtBearerEvents
    //         // {
    //         //     OnMessageReceived = context =>
    //         //     {
    //         //         var accessToken = context.Request.Query["access_token"];
    //         //         var path = context.HttpContext.Request.Path;
    //         //         if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
    //         //         {
    //         //             context.Token = accessToken;
    //         //         }
    //         //         return Task.CompletedTask;
    //         //     }
    //         // };
    //         var tokenKey = Environment.GetEnvironmentVariable("TokenKey") ?? config["TokenKey"];

    //     if (string.IsNullOrWhiteSpace(tokenKey) || tokenKey.Length < 32)
    //         throw new Exception("TokenKey is missing or too short. Must be at least 32 characters.");

    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

    //     services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //         .AddJwtBearer(options =>
    //         {
    //             options.TokenValidationParameters = new TokenValidationParameters
    //             {
    //                 ValidateIssuerSigningKey = true,
    //                 IssuerSigningKey = key,
    //                 ValidateIssuer = false,
    //                 ValidateAudience = false
    //             };
    //             options.Events = new JwtBearerEvents
    //             {
    //                 OnMessageReceived = context =>
    //                 {
    //                     var accessToken = context.Request.Query["access_token"];
    //                     var path = context.HttpContext.Request.Path;
    //                     if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
    //                     {
    //                         context.Token = accessToken;
    //                     }
    //                     return Task.CompletedTask;
    //                 }
    //             };
    //         });

    //     });

    //     services.AddAuthorizationBuilder()
    //         .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
    //         .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
    //     return services;
    // }
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
{
    services.AddIdentityCore<AppUser>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<AppRole>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddEntityFrameworkStores<DataContext>();

    var tokenKey = Environment.GetEnvironmentVariable("TokenKey") ?? config["TokenKey"];

    if (string.IsNullOrWhiteSpace(tokenKey) || tokenKey.Length < 32)
        throw new Exception("TokenKey is missing or too short. Must be at least 32 characters.");

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

    services.AddAuthorizationBuilder()
        .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
        .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));

    return services;
}

}


// public static class IdentityServiceExtensions
// {
//     public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
//     {
//         services.AddIdentityCore<AppUser>(opt =>
//         {
//             opt.Password.RequireNonAlphanumeric = false;
//         })
//         .AddRoles<AppRole>()
//         .AddRoleManager<RoleManager<AppRole>>()
//         .AddEntityFrameworkStores<DataContext>();

//         var tokenKey = Environment.GetEnvironmentVariable("TokenKey") ?? config["TokenKey"];

//         if (string.IsNullOrWhiteSpace(tokenKey) || tokenKey.Length < 32)
//             throw new Exception("TokenKey is missing or too short. Must be at least 32 characters.");

//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

//         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//             .AddJwtBearer(options =>
//             {
//                 options.TokenValidationParameters = new TokenValidationParameters
//                 {
//                     ValidateIssuerSigningKey = true,
//                     IssuerSigningKey = key,
//                     ValidateIssuer = false,
//                     ValidateAudience = false
//                 };
//                 options.Events = new JwtBearerEvents
//                 {
//                     OnMessageReceived = context =>
//                     {
//                         var accessToken = context.Request.Query["access_token"];
//                         var path = context.HttpContext.Request.Path;
//                         if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
//                         {
//                             context.Token = accessToken;
//                         }
//                         return Task.CompletedTask;
//                     }
//                 };
//             });

//         services.AddAuthorizationBuilder()
//             .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
//             .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));

//         return services;
//     }
// }
