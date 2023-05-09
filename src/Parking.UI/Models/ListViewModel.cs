using ApplicationCore.Entities;

namespace Parking.UI.Models
{
    public sealed class ListViewModel
    {
        public List<ParkIn>? ParkIns { get; set; }

        public int SpotTaken { get; set; }
    }
}
