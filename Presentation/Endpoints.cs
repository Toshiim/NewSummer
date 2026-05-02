using Application.Common.Interfaces;
using Application.Common.Models;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;


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

        app.MapGet("/articles", async (GetArticlesUseCase useCase, [AsParameters] GetArticlesRequest request, CancellationToken ct) =>
            {
                var query = new GetArticlesQuery(
                    Page: request.Page ?? 1,
                    PageSize: request.PageSize ?? 20,
                    SortBy: request.SortBy ?? SortBy.DateAdded,
                    SortOrder: request.SortOrder ?? SortOrder.Desc,
                    SourceId: request.SourceId,
                    CategoryId: request.CategoryId,
                    SourceName: request.SourceName
                );

                return await useCase.GetArticles(query, ct);
            })
            .WithName("GetArticles")
            .WithOpenApi();

        app.MapPost("/sources", async (CreateSourceUseCase useCase, [FromBody] CreateSourceCommand command, CancellationToken ct) =>
        {
            return await useCase.CreateSource(command, ct);
        });

        app.MapGet("/sources", async (GetSourcesUseCase useCase, CancellationToken ct) => await useCase.GetSources(ct));
    }
}