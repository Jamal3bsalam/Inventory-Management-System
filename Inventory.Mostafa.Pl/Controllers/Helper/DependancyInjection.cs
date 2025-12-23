using Inventory.Mostafa.Application.Abstraction.Cash;
using Inventory.Mostafa.Application.Abstraction.DataBase;
using Inventory.Mostafa.Application.Abstraction.Files;
using Inventory.Mostafa.Application.Abstraction.Token;
using Inventory.Mostafa.Application.Abstraction.UnitOfWork;
using Inventory.Mostafa.Application.User.Command.LogIn;
using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Infrastructure.Data.Context;
using Inventory.Mostafa.Infrastructure.Data.Repositories;
using Inventory.Mostafa.Infrastructure.Data.Seed;
using Inventory.Mostafa.Infrastructure.Service.Cashe;
using Inventory.Mostafa.Infrastructure.Service.DataBase;
using Inventory.Mostafa.Infrastructure.Service.Files;
using Inventory.Mostafa.Infrastructure.Service.Token;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory.Mostafa.Pl.Controllers.Helper
{
    public static class DependancyInjection
    {

        public static IServiceCollection AddDependancies(this IServiceCollection services, IConfiguration configuration)
        {
            AddInfrastructureServices(services, configuration);
            AddUserDefineServices(services);
            SwaggerConfiguration(services);
            JwtAuthConfiguration(services, configuration);
            SolveCORSProblem(services);
            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }

        public static IServiceCollection AddUserDefineServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddMapster();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IFileServices<,>), typeof(FilesServices<,>));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LogInCommandHandler).Assembly);
            });
            services.AddMemoryCache();
            services.AddScoped<ICashService, CasheService>();
            services.AddScoped<IDataBaseBackUpService, DataBaseBackUpService>();
            return services;
        }

        public static IServiceCollection SwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory Management Systme API", Version = "v1" });
                c.UseInlineDefinitionsForEnums();


                // Define the security scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Apply the security requirement globally
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    });
            });

            

            return services;
        }

        public static IServiceCollection JwtAuthConfiguration(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
                };
            });

            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.User.AllowedUserNameCharacters =
                     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._ ";
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
            })
           .AddEntityFrameworkStores<InventoryDbContext>()
           .AddDefaultTokenProviders();

            return services;
        }

        public async static Task UseDatabaseMigrationAndSeedAsync(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<InventoryDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            try
            {
                // Update DataBase && Apply Migrations
                await context.Database.MigrateAsync();
                await InventoryDbSeed.SeedRoles(roleManager);
                await InventoryDbSeed.SeedAppUser(userManager);

            }
            catch (Exception ex)
            {
                var loggerfactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "there are problems during apply migrations");
            }
        }

        public static IServiceCollection SolveCORSProblem(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}
