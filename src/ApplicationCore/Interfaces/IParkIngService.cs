using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IParkingService
    {
        public Task<bool> CheckIn(string tagNumber);

        public Task<bool> CheckOut(string tagNumber, decimal hourlyFee, decimal total);

        public Task<ParkIn> ParkInByTagNumber(string tagNumber);

        public Task<bool> IsCarAlreadyParked(string tagNumber);

        public Task<int> AvailableSpot(int maxSpot);

        public Task<bool> AnySpotAvailable(int maxSpot);

        public Task<List<ParkIn>> ParksInAsync();

        public Task<decimal> RevenueOfTheDay(DateTime dateTime, decimal hourlyFee);


        /// <summary>
        /// Calculate Avarage number of car for the date interval 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public Task<int> AverageNumberOfCar(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Calculate Avarage Revenue for the date interval 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="hourlyFee"></param>
        /// <returns></returns>
        public Task<decimal> AverageRevenue(DateTime fromDate, DateTime toDate, decimal hourlyFee);
    }
}
