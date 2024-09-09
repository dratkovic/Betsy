namespace Betsy.Domain.Common;

public abstract class EntityBase
{
    protected readonly List<IDomainEvent> _domainEvents = [];

    public Guid Id { get; init; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    protected EntityBase(Guid id) => Id = id;
    protected EntityBase() { }

    public IList<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();

        _domainEvents.Clear();

        return copy;
    }
}
