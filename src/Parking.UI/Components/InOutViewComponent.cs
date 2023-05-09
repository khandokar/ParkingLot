using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Enums;
using Parking.UI.Models;

namespace Parking.UI.ViewComponents
{
    public class InOutViewComponent : ViewComponent
    {
        private InOutViewModel? inOutViewModel;
        private readonly IParkingService parkingService;
        private readonly IConfiguration config;
        
        public InOutViewComponent(IParkingService parkingService, IConfiguration config)
        {
            this.parkingService = parkingService;
            this.config = config;
            inOutViewModel = new InOutViewModel();
        }

        public IViewComponentResult Invoke(InOutViewModel? inOutViewModel)
        {
            this.inOutViewModel = inOutViewModel == null ? new InOutViewModel() { Type = InOutType.In} : inOutViewModel;

            if(this.inOutViewModel.Type == InOutType.OutValidated)
            {
                ViewBag.DisabledTagNumber = true;
                ParkIn parkIn = parkingService.ParkInByTagNumber(inOutViewModel.TagNumber).Result;
                decimal hourlyFee = config.GetValue<int>("HourlyFee");

                TimeSpan timeSpan = DateTime.Now.Subtract(parkIn.CheckIn);

                int hour = Convert.ToInt32(Math.Truncate(timeSpan.TotalHours));
                decimal minute = 0;
                if (hour > 0)
                {
                    //minute = Math.Round(Convert.ToDecimal(timeSpan.TotalHours - hour) * 100, 0);
                    minute = timeSpan.Minutes;
                }
                else
                {
                    minute = Math.Round(Convert.ToDecimal(timeSpan.TotalMinutes));
                }

                this.inOutViewModel.ElaspedTime = string.Format("{0} hr {1} min", hour, minute);

                this.inOutViewModel.Total = Math.Round(Math.Ceiling(Convert.ToDecimal(timeSpan.TotalHours)) * hourlyFee, 2);
            }  
            return View(this.inOutViewModel);
        }
    }
}
