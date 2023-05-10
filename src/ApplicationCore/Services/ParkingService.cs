using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;

namespace ApplicationCore.Services
{
    public class ParkingService : IParkingService
    {
        private readonly IRepository<ParkIn> parkInRepository;
        private readonly IRepository<ParkOut> parkOutRepository;
        private readonly ICommonRepository commonRepository;
        private readonly IDbManager dbManager;

        private  Specification<ParkIn>? specificationParkInByTagName { get;}

        public ParkingService(IRepository<ParkIn> parkInRepository, IRepository<ParkOut> parkOutRepository, ICommonRepository commonRepository) 
        {
            this.parkInRepository = parkInRepository;
            this.parkOutRepository = parkOutRepository;
            this.commonRepository = commonRepository;
            this.dbManager = parkInRepository.DbManager;
        }

        public ParkingService(IRepository<ParkIn> parkInRepository, IRepository<ParkOut> parkOutRepository, ICommonRepository commonRepository, 
                              Specification<ParkIn> specificationParkInByTagName, IDbManager dbManager = null)
        {
            this.parkInRepository = parkInRepository;
            this.parkOutRepository = parkOutRepository;
            this.commonRepository = commonRepository;
            this.specificationParkInByTagName = specificationParkInByTagName;
            this.dbManager = dbManager;
        }

        public async Task<bool> CheckIn(string tagNumber)
        {
            try
            {
                ParkIn parkIn = new ParkIn(tagNumber, DateTime.Now);

                await parkInRepository.AddAsync(parkIn);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckOut(string tagNumber, decimal hourlyFee, decimal total)
        {
            try
            {
                ParkIn parkIn = await ParkInByTagNumber(tagNumber);

                if (parkIn == null)
                {
                    throw new Exception("Car with this Tag number Not Found.");
                }

                ParkOut parkOut = new ParkOut(tagNumber, parkIn.CheckIn, DateTime.Now, hourlyFee, total);

                dbManager.OpenTransaction();

                try
                {
                    await parkInRepository.DeleteAsync(parkIn);

                    await parkOutRepository.AddAsync(parkOut);

                    dbManager.CommitTransaction();
                }
                catch (Exception)
                {
                    dbManager.RollbackTransaction();
                    throw;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ParkIn> ParkInByTagNumber(string tagNumber)
        {
            try
            {
                Specification<ParkIn> getParkInByTagNumber = specificationParkInByTagName ?? new ParkInByTagNumber(tagNumber);

                List<ParkIn> parkIns = await parkInRepository.GetAllAsync(getParkInByTagNumber);

                if (parkIns.Any())
                    return parkIns[0];

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsCarAlreadyParked(string tagNumber)
        {
            try
            {
                Specification<ParkIn> getParkInByTagNumber = specificationParkInByTagName ?? new ParkInByTagNumber(tagNumber);

                bool isParked = await parkInRepository.AnyAsync(getParkInByTagNumber);

                return isParked;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AnySpotAvailable(int maxSpot)
        {
            try
            {
                int totalCar = await parkInRepository.CountAsync(null);

                return maxSpot > totalCar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> AvailableSpot(int maxSpot)
        {
            try
            {
                int totalCar = await parkInRepository.CountAsync(null);

                return maxSpot - totalCar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ParkIn>> ParksInAsync()
        {
            try
            {
                return await parkInRepository.GetAllAsync(null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> RevenueOfTheDay(DateTime dateTime, decimal hourlyFee)
        {
            try
            {
                decimal total = await commonRepository.RevenueOfTheDay(dateTime, hourlyFee);

                return total;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> AverageNumberOfCar(DateTime fromDate, DateTime toDate)
        {
            try
            {     
                int count = await commonRepository.AverageNumberOfCar(fromDate, toDate);

                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }
     
        public async Task<decimal> AverageRevenue(DateTime fromDate, DateTime toDate, decimal hourlyFee)
        {
            try
            {
                decimal total = await commonRepository.AverageRevenue(fromDate, toDate, hourlyFee);

                return total;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
