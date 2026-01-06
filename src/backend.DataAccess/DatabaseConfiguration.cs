using backend.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace backend.DataAccess
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? appDbContext = configuration.GetConnectionString("AppDbContext");
            if (appDbContext == null)
            {
                throw new Exception("AppDbContext connection string not found");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(appDbContext);
            });
        }
    }
}
