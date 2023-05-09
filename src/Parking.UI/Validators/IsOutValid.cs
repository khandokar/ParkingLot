using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Parking.UI.Models;
using System.ComponentModel.DataAnnotations;

namespace Parking.UI.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsOutValid : ValidationAttribute
    {
        public IsOutValid() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Tag Number is required.");
            }

            var model = (InOutViewModel)validationContext.ObjectInstance;

            if (model.Type == InOutType.In || model.Type == InOutType.OutValidated)
            {
                return ValidationResult.Success;
            }

            IParkingService? parkingService = validationContext.GetService(typeof(IParkingService)) as IParkingService;

            if (parkingService == null)
            {
                return new ValidationResult("Failed to Validate.");
            }

            bool isParked = parkingService.IsCarAlreadyParked(value.ToString()).Result;

            if (!isParked)
            {
                return new ValidationResult("This Car is not in the Park.");
            }

            return ValidationResult.Success;

        }
    }
}
