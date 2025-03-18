
using CountryUniversities.Database;
using CountryUniversities.Repositories;
using CountryUniversities.Services;
using Microsoft.OpenApi.Models;

namespace CountryUniversities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
#if DEBUG
            builder.Configuration.AddJsonFile("Properties/appsettings.Development.json", optional: false,
                reloadOnChange: true);
#else
            builder.Configuration.AddJsonFile("Properties/appsettings.json", optional: false, reloadOnChange: true);
#endif
            
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<DatabaseConnection>();
            builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();

            builder.Services.AddScoped<IUniversityService, UniversityService>();

            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });


            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization(); 
            app.UseCors("AllowAll");

            app.MapControllers();
            app.Run();
        }
    }
}
