
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Api.Filters;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Mappings;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Services.Interfaces;
using SocialMedia.Services.Services;
using SocialMedia.Services.Validators;

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
            //builder.Services.AddTransient<IPostRepository, PostRepository>();
            //builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IPostService, PostService>();
            builder.Services.AddScoped
                (typeof(IBaseRepository<>),
                (typeof(BaseRepository<>)));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(
                options =>
                { 
                    options.SerializerSettings.ReferenceLoopHandling
                     = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
             ).ConfigureApiBehaviorOptions(options =>
             {
                options.SuppressModelStateInvalidFilter = true;
             });

            //Configurar Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Backend Social Media API",
                    Version = "v1",
                    Description = "Documentación de la API de Social Media .NET 9",
                    Contact = new()
                    {
                        Name = "Equipo de desarrollo UCB",
                        Email = "desarrollo@ucb.edu.bo"
                    }
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                //configurar los parametros de objeto
                options.EnableAnnotations();
            });

            //Configurar JWT
            builder.Services.AddAuthentication(options =>
            {
                //Esquema por defecto para autenticar (identificar quien es el usuario)
                //Se va usar JWT Bearer como estándar
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                //si alguien intenta acceder si entar autenticado, se tiene que bloquear
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    //Valida el Emisor (iss) > Verifica que el token haya
                    //sido emitido por un servidor de confianza, Evitar que alguien use
                    //Tokens creados por otros sistemas
                    ValidateIssuer = true,

                    //Verifica que el token este dirigido a una API en particular
                    //un servicio de frontend, un cliente
                    ValidateAudience = true,

                    //Comprueba que el token no haya expirado
                    ValidateLifetime = true,

                    //Verifica que el token no haya sido modificada
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],

                    //Clave simétrica, esta misma sirve para firmar y verificar
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                            builder.Configuration["Authentication:SecretKey"])
                    )
                };
            });


            //Registra el profile del automapper para el Post
            builder.Services.AddAutoMapper(typeof(PostProfile).Assembly);

            //Registrar Validadores de FluentValidations
            builder.Services.AddScoped<PostDtoValidator>();
            builder.Services.AddScoped<CrearPostDtoValidator>();
            builder.Services.AddScoped<ActualizarPostDtoValidator>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            //Usar Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Social Media API v1");
                    options.RoutePrefix = string.Empty; //Swagger sera accesible en la raíz
                });
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
