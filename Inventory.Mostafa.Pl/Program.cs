using Inventory.Mostafa.Domain.Entities.Identity;
using Inventory.Mostafa.Infrastructure.Data.Context;
using Inventory.Mostafa.Infrastructure.Data.Seed;
using Inventory.Mostafa.Pl.Controllers.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Inventory.Mostafa.Pl
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers()
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                   });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDependancies(builder.Configuration);

            var app = builder.Build();
            app.UseCors("AllowAngular");


            await app.UseDatabaseMigrationAndSeedAsync();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AncientAura API V1");
                    options.RoutePrefix = ""; // ÌÃ⁄· Swagger «·’›Õ… «·«› —«÷Ì…
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();

        }
    }
}
