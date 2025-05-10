using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.Config
{
    public static class AuthorizationConfig
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "sua-api",
                        ValidAudience = "seus-clientes",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes("sua-chave-super-secreta"))
                    };
                });

            services.AddAuthorization(options =>
            {
                // Global Policy: requires minimum "read" permission
                options.AddPolicy("RequireRead", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c =>
                            c.Type == "permission" &&
                            (c.Value == "read" || c.Value == "write" || c.Value == "all"))
                    ));

                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("CanWrite", policy => policy.RequireClaim("permission", "write"));
            });
        }

        public static void EnableAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
