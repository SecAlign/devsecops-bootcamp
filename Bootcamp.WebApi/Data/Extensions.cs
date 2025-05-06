// // Created On: 2025.05.06
// // Create by: althunibat

using Bootcamp.WebApi.Config;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.WebApi.Data;

public static class Extensions {

    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration) {
        var dbConn = configuration.GetDbConnection();
        services.AddDbContextPool<ApiDbContext>(options => {
            options
                .UseNpgsql(dbConn, npgsql => {
                    npgsql
                        .UseAdminDatabase("postgres")
                        .MigrationsAssembly(typeof(ApiDbContext).Assembly)
                        .EnableRetryOnFailure();
                }).EnableDetailedErrors();
            options.UseSnakeCaseNamingConvention();
        });
        services.AddHostedService<MigrationsService>();
        return services;
    }
}