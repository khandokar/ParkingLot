using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Parking.UI.Models;
using System.ComponentModel.DataAnnotations;

namespace Parking.UI.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsInValid : ValidationAttribute
    {
        public IsInValid() { }


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Tag Number is required.");
            }

            var model = (InOutViewModel)validationContext.ObjectInstance;

            if (model.Type == InOutType.Out || model.Type == InOutType.OutValidated)
            {
                return ValidationResult.Success;
            }

            IParkingService? parkingService = validationContext.GetService(typeof(IParkingService)) as IParkingService;
            IConfiguration? config = validationContext.GetService(typeof(IConfiguration)) as IConfiguration;

            if (parkingService == null)
            {
                return new ValidationResult("Failed to Validate.");
            }

            int totalSpot = config.GetValue<int>("TotalSpot");

            bool isAvailAble = parkingService.AnySpotAvailable(totalSpot).Result;
            if (!isAvailAble)
            {
                return new ValidationResult("Spot is not available.");
            }

            bool isParked = parkingService.IsCarAlreadyParked(value.ToString()).Result;

            if (isParked)
            {
                return new ValidationResult("This Car is already Parked.");
            }

            return ValidationResult.Success;
        }
    }
}
