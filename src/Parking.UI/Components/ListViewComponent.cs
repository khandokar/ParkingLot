using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Parking.UI.Models;

namespace Parking.UI.Components
{
    public class ListViewComponent : ViewComponent
    {
        private readonly IParkingService parkingService;
        private readonly IConfiguration config;
        public ListViewComponent(IParkingService parkingService, IConfiguration config)
        {
            this.parkingService = parkingService;
            this.config = config;
        }

        public IViewComponentResult Invoke()
        {
            int totalSpot = config.GetValue<int>("TotalSpot");
            decimal hourlyFee = config.GetValue<int>("HourlyFee");
            ViewBag.TotalSpot = totalSpot;
            ViewBag.HourlyFee = hourlyFee;

            ListViewModel listViewModel = new ListViewModel();
            List<ParkIn> parkins = parkingService.ParksInAsync().Result;
            listViewModel.ParkIns = parkins;
            listViewModel.SpotTaken = parkins.Count;

            return View(listViewModel);
        }
    }
}
