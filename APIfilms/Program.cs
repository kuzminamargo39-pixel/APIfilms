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

            // Add services to the container.

            builder.Services.AddScoped<IDataRepository<Utilisateur>, UtilisateurManager>();
            builder.Services.AddScoped<IDataRepository<Film>, FilmManager>();
            builder.Services.AddScoped<IDataRepository<Notation>, NotationManager>();

            builder.Services.AddControllers();

            builder.Services.AddDbContext<FilmRatingsDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
