
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Mappings;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configurar la BD SqlServer
            //var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            //builder.Services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(connectionString));
            #endregion

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<SocialMediaContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion
            
            //Registrar los servicios
            builder.Services.AddTransient<IPostRepository, PostRepository>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(
                options =>
                { 
                    options.SerializerSettings.ReferenceLoopHandling
                     = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
             );

            //Registra el profile del automapper para el Post
            builder.Services.AddAutoMapper(typeof(PostProfile).Assembly);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
