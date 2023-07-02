using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors();

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            builder.Services.AddTransient<IGenresService, GenresService>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().WithOrigins());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}