namespace Shared.Kernel.Abstractions
{
    public interface IDomainEvent
    {
        public Guid IdEvent { get; }
        public DateTime OccurredOnUtc { get; }
    }
}