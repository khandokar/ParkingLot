using Parking.UI.Models;
using System.ComponentModel.DataAnnotations;
using ApplicationCore.Enums;

namespace Parking.UI.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsAmountValid : ValidationAttribute
    {
        public IsAmountValid()
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (InOutViewModel)validationContext.ObjectInstance;

            if (model.Type == InOutType.OutValidated)
            {
                bool success = int.TryParse(value?.ToString(), out int number);
                if (!success)
                {
                    return new ValidationResult("This field is must be a number.");
                }

                if (Convert.ToDecimal(value) < 0)
                {
                    return new ValidationResult("This field is must be positive.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
