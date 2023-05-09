using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Parking.UI.Models;
using ApplicationCore.Enums;

namespace Parking.UI.Controllers
{
    public class ParkingController : Controller
    {
        private readonly IParkingService parkingService;
        private readonly IConfiguration config;

        public ParkingController(IParkingService parkingService, IConfiguration config)
        {
            this.parkingService = parkingService;
            this.config = config;
        }

        public IActionResult In()
        {
            return ViewComponent("InOut");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> In(InOutViewModel parkViewModel)
        {
            if (!ModelState.IsValid)
            {
                return ViewComponent("InOut", parkViewModel);
            }

            await parkingService.CheckIn(parkViewModel.TagNumber);
            ModelState.Clear();

            return ViewComponent("InOut");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Out(InOutViewModel parkViewModel)
        {

            if (!ModelState.IsValid)
            {
                return ViewComponent("InOut", parkViewModel);
            }

            if (parkViewModel.Type == InOutType.Out)
            {
                parkViewModel.Type = InOutType.OutValidated;
                ModelState.Clear();
                return ViewComponent("InOut", parkViewModel);
            }

            decimal hourlyFee = config.GetValue<int>("HourlyFee");
            await parkingService.CheckOut(parkViewModel.TagNumber, hourlyFee, parkViewModel.Total);
            ModelState.Clear();

            return ViewComponent("InOut");

        }

        public IActionResult Spots()
        {
            return ViewComponent("List");
        }

        public IActionResult Stats()
        {
            return ViewComponent("Stats");
        }
    }
}
