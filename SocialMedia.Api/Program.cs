using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.DTOs;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Mappings;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Validators;

namespace SocialMedia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD SqlServer
            //var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            //builder.Services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(connectionString));
            #endregion

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<SocialMediaContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Inyectar las dependencias
            //builder.Services.AddTransient<IPostRepository, PostRepository>();
            builder.Services.AddTransient<IPostService, PostService>();
            //builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //Validaciones
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            //Configuracion de Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Backend Social Media API",
                    Version = "v1",
                    Description = "Documentacion de la API de Social Media - NET 9",
                    Contact = new()
                    {
                        Name = "Equipo de Desarrollo UCB",
                        Email = "desarrollo@ucb.edu.bo"
                    }
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.EnableAnnotations();
            });

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<PostDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetByIdRequestValidator>();

            // Services
            builder.Services.AddScoped<IValidationService, ValidationService>();

            var app = builder.Build();

            //Usar Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Social Media API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
