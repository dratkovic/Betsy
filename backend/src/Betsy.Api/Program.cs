using Betsy.Api;
using Betsy.Api.Middlewares;
using Betsy.Application;
using Betsy.Infrastructure.Common.Middleware;
using Betsy.Infrastructure.Common.Persistence;
using Betsy.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Betsy.Api.Endpoints.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddDb();

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// For now all requests that contain Authorization attribute are not cached by design
// TODO: need to implement cache policy
// that will allow to cache requests with Authorization attribute as api consumers usually 
// put it in all requests
builder.AddRedisOutputCache("betsy-cache");

// Add services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler();

app.AddInfrastructureMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MigrateDatabase();

app.UseOutputCache();

app.UseMetricsCacheDisabler();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints<IBetsyApiMarker>();

app.Run();