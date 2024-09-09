using System.Reflection;
using Betsy.Application.Common.Interfaces;
using Betsy.Domain.Common;
using Betsy.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Betsy.Infrastructure.Common.Persistence;

public class BetsyDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    private readonly IUserSession _userSession;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // used both approaches to show the difference
    public DbSet<User> Users { get; set; } = null!;

    // used generic approach to show the difference
    public DbSet<T> GetDbSet<T>() where T : EntityBase
    {
        return Set<T>();
    }

    public BetsyDbContext(DbContextOptions options,
        IHttpContextAccessor httpContextAccessor,
        IPublisher publisher,
        IUserSession userSession
        ) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _publisher = publisher;
        _userSession = userSession;
    }

    public async Task CommitChangesAsync(CancellationToken token)
    {
        // get hold of all the domain events
        var domainEvents = ChangeTracker.Entries<EntityBase>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        // store them in the http context for later if user is waiting online
        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            await PublishDomainEvents(_publisher, domainEvents);
        }

        await SaveChangesAsync(token);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _userSession.GetCurrentUser().Id;
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _userSession.GetCurrentUser().Id;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = _userSession.GetCurrentUser().Id;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    private static async Task PublishDomainEvents(IPublisher _publisher, List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        // fetch queue from http context or create a new queue if it doesn't exist
        var domainEventsQueue = _httpContextAccessor.HttpContext!.Items
            .TryGetValue("DomainEventsQueue", out var value) && value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new Queue<IDomainEvent>();

        // add the domain events to the end of the queue
        domainEvents.ForEach(domainEventsQueue.Enqueue);

        // store the queue in the http context
        _httpContextAccessor.HttpContext!.Items["DomainEventsQueue"] = domainEventsQueue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}