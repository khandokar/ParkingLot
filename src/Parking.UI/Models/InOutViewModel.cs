using ApplicationCore.Enums;
using Parking.UI.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Parking.UI.Models
{
    public sealed class InOutViewModel
    {
        public InOutViewModel() 
        {
            Type = InOutType.In;
            Total= 0;
            ElaspedTime = string.Empty;
        }
        
        [DisplayName("Tag Number:")]
        [Required(ErrorMessage = "Tag Number is required.")]
        [StringLength(10, ErrorMessage = "Tag Number should be more than {0} characters.")]
        [IsInValid]
        [IsOutValid]
        public string TagNumber { get; set; }

        [DisplayName("Total:")]
        [IsAmountValid]
        public decimal Total { get; set; }

        [DisplayName("Elasped Time:")]
        public string ElaspedTime { get; set; }

        public InOutType Type { get; set; }
    }


}
