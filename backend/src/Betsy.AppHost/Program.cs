using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


// * Prometheus *
builder.AddContainer("betsy-prometheus", "prom/prometheus")
    .WithBindMount("../prometheus", "/etc/prometheus", isReadOnly: true)
    .WithHttpEndpoint(/* This port is fixed as it's referenced from the Grafana config */ port: 9090, targetPort: 9090);

// * Grafana *
var grafana = builder.AddContainer("betsy-grafana", "grafana/grafana")
    .WithBindMount("../grafana/config", "/etc/grafana", isReadOnly: true)
    .WithBindMount("../grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
    .WithHttpEndpoint(targetPort: 3000, name: "http");

// * Redis *
var redis = builder.AddRedis("betsy-cache");


// * SQL Server *
var sql = builder.AddSqlServer("betsy-sql-server")
    .WithBindMount("../sqlserver/data/data", "/var/opt/mssql/data")
    .WithBindMount("../sqlserver/data/log", "/var/opt/mssql/log");

var sqlDb = sql.AddDatabase("betsy-db");

// * API *
builder.AddProject<Projects.Betsy_Api>("betsy-api")
    .WithReference(redis)
    .WithReference(sqlDb)
    .WaitFor(sqlDb)
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("http"));

await using var app = builder.Build();

await app.RunAsync();
