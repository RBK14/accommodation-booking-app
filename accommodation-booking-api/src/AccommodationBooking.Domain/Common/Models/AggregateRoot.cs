namespace AccommodationBooking.Domain.Common.Models
{
    /// <summary>
    /// Base class for aggregate roots in DDD pattern.
    /// </summary>
    /// <typeparam name="TId">The type of the aggregate root identifier.</typeparam>
    public abstract class AggregateRoot<TId> : Entity<TId>
        where TId : notnull
    {
        protected AggregateRoot(TId id) : base(id)
        {
        }

#pragma warning disable CS8618
        protected AggregateRoot()
        {
        }
#pragma warning restore CS8618
    }
}
