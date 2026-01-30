namespace AccommodationBooking.Application.Common.Interfaces.Persistence
{
    /// <summary>
    /// Interface for implementing query filters.
    /// </summary>
    /// <typeparam name="T">The entity type to filter.</typeparam>
    public interface IFilterable<T>
    {
        /// <summary>
        /// Applies the filter to the query.
        /// </summary>
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
