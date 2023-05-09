namespace ApplicationCore.Interfaces
{
    public interface ICommonRepository
    {
        public Task<decimal> RevenueOfTheDay(DateTime dateTime, decimal hourlyFee, CancellationToken cancellationToken = default);

        public Task<int> AverageNumberOfCar(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

        public Task<decimal> AverageRevenue(DateTime fromDate, DateTime toDate, decimal hourlyFee, CancellationToken cancellationToken = default);
    }
}
