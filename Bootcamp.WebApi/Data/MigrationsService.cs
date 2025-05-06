// // Created On: 2025.05.06
// // Create by: althunibat

using Microsoft.EntityFrameworkCore;

namespace Bootcamp.WebApi.Data;

public class MigrationsService(IServiceScopeFactory serviceScopeFactory):IHostedService {
    public async Task StartAsync(CancellationToken cancellationToken) {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        await db.Database.MigrateAsync(cancellationToken);
    }
    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}