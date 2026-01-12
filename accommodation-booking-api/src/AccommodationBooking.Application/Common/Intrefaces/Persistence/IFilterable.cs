namespace AccommodationBooking.Application.Common.Intrefaces.Persistence
{
    public interface IFilterable<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }

}
