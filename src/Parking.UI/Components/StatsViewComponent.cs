using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Parking.UI.Models;

namespace Parking.UI.Components
{
    public class StatsViewComponent : ViewComponent
    {
        private readonly IParkingService parkingService;
        private readonly IConfiguration config;
        public StatsViewComponent(IParkingService parkingService, IConfiguration config)
        {
            this.parkingService = parkingService;
            this.config = config;
        }

        public IViewComponentResult Invoke()
        {
            StatsViewModel statsViewModel = new StatsViewModel();
            int totalSpot = config.GetValue<int>("TotalSpot");
            int hourlyFee = config.GetValue<int>("HourlyFee");

            DateTime toDate = DateTime.Now.AddDays(-1);
            DateTime fromDate = toDate.AddDays(-30);


            statsViewModel.AvailableSpot = parkingService.AvailableSpot(totalSpot).Result;
            statsViewModel.AverageCarPerDay = parkingService.AverageNumberOfCar(fromDate, toDate).Result;
            statsViewModel.AverageRevenuePerDay = parkingService.AverageRevenue(fromDate, toDate, hourlyFee).Result;
            statsViewModel.TodayRevenue = parkingService.RevenueOfTheDay(DateTime.Now, hourlyFee).Result;

            return View(statsViewModel);
        }
    }
}
