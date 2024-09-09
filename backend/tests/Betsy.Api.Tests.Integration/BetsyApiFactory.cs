using System.Data.Common;
using Betsy.Application.Common.Cache;
using Betsy.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using Testcontainers.MsSql;

namespace Betsy.Api.Tests.Integration;

public class BetsyApiFactory : WebApplicationFactory<IBetsyApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = null!;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

            services.RemoveAll(typeof(IOutputCacheStore));
            services.RemoveAll(typeof(ICacheService));
            services.RemoveAll(typeof(BetsyDbContext));
            services.RemoveAll(typeof(DbContextOptions));

            // ** Tricky part **
            services.RemoveAll(typeof(IDbContextPool<BetsyDbContext>));
            services.RemoveAll(typeof(IScopedDbContextLease<BetsyDbContext>));
            // needed to figure this out.. Aspire pooling

            services.RemoveAll(typeof(DbContextOptions<BetsyDbContext>));

            services.AddSingleton<IOutputCacheStore, FakeOutputCacheStore>();
            services.AddSingleton<ICacheService, FakeCacheService>();
            services.AddDbContext<BetsyDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString());
            });
        });

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders(); // Optional: Clear other log providers if needed
            logging.AddConsole(); // Optional: Add console logging
        });

        builder.UseEnvironment("Development");
    }

    public void ResetHttpClient()
    {
        HttpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task ResetDataBase()
    {
      //  await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        await EnsureDatabaseCreatedAsync();

        HttpClient = CreateClient();

       // await InitializeRespawner();
    }

    public Task DisposeAsync()
    {
        return _msSqlContainer.DisposeAsync().AsTask();
    }

    private async Task InitializeRespawner()
    {
        _dbConnection = new SqlConnection(_msSqlContainer.GetConnectionString());

        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToInclude = []
        });
    }

    private async Task EnsureDatabaseCreatedAsync()
    {
        var options = new DbContextOptionsBuilder<BetsyDbContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString())
            .Options;

        using var context = new BetsyDbContext(options, null!, null!, null!);
        await context.Database.EnsureCreatedAsync();
    }
}
