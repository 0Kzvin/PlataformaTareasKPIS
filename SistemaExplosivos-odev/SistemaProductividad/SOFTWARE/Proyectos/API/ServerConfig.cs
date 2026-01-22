using API.Database.Core;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public static class ServerConfig
    {
        public static async Task ConfigurarServer(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<SistemaProductividadContext>();
                    
                    // Apply migrations
                    if ((await context.Database.GetPendingMigrationsAsync()).Any())
                    {
                        await context.Database.MigrateAsync();
                    }
                    
                    // Seed initial data (Admin user, etc.) - To be implemented
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error configuring server: {e.Message}");
                }
            }
        }
    }
}
