using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Tls;

namespace backend.Recycle.Extensions.Services
{
    public static class ServiceCollection
    {
       
        public static IServiceCollection AddSwagger(this IServiceCollection service)
        {
            service.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return service;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection service,IConfiguration config)
        {
            //service.AddAuthorization(o =>
            //{
            //    o.AddPolicy("employee",p=>p.RequireRole("employee"));
            //});
            service.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return service;
        }

        public static async Task AddRoleAsync(IServiceProvider service)
        {
            var role=service.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var item in Roles.Role)
            {
                if (!await role.RoleExistsAsync(item))
                {
                    await role.CreateAsync(new IdentityRole(item));
                }
            }
           
        } 
    }
}
