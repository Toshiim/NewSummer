using Application.Interfaces;


namespace Presentation;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", async (IDatabaseHealthService healthService) =>
            {
                var isHealthy = await healthService.IsDatabaseHealthyAsync();
                
                if (isHealthy)
                {
                    return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
                }

                return Results.Json(
                    new { status = "unhealthy", timestamp = DateTime.UtcNow },
                    statusCode: 503);
            })
            .WithName("Health")
            .WithOpenApi();
    }
    
}