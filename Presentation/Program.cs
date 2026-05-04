using Application;
using Application.UseCases;
using Hangfire;
using Infrastructure;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new MyAuthorizationFilter() }
    });
}


RecurringJob.AddOrUpdate<ScrapArticleUseCase>(
    "scrape-news",
    job => job.ExecuteAsync(CancellationToken.None),
    Cron.Never); 


app.MapEndpoints();
app.UseHttpsRedirection();

app.Run();
