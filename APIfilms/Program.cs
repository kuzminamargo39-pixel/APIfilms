using APIfilms.Models.DataManager;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace APIfilms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IDataRepository<Utilisateur>, UtilisateurManager>();
            builder.Services.AddScoped<IDataRepository<Film>, FilmManager>();
            builder.Services.AddScoped<IDataRepository<Notation>, NotationManager>();

            builder.Services.AddControllers();

            builder.Services.AddDbContext<FilmRatingsDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorOrigin", policy =>
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowBlazorOrigin");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}