using Microsoft.EntityFrameworkCore;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD SqlServer
            var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            builder.Services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(connectionString));
            #endregion

            #region Configurar la BD MySql
            //var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            //builder.Services.AddDbContext<SocialMediaContext>(options =>
            //    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion


            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
