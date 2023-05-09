namespace Parking.UI.Models
{
    public sealed class StatsViewModel
    {
        public int AvailableSpot { get; set; }

        public decimal TodayRevenue { get; set; }

        public int AverageCarPerDay { get; set; }

        public decimal AverageRevenuePerDay { get; set; }
    }
}
