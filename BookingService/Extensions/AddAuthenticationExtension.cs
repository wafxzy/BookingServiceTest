using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookingService.Extensions
{
    public static class AddAuthenticationExtension
    {
        //public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        //{

        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = configuration["JWT:ValidIssuer"],
        //            ValidAudience = configuration["JWT:ValidAudience"],
        //            IssuerSigningKey = new SymmetricSecurityKey(
        //                Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!))
        //        };
        //    });
        //    return services;
        //}
    }
}
