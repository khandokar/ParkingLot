namespace ApplicationCore.Entities
{
    public class ParkOut : BaseEntity
    {
        public DateTime InsertTime { get; set; }

        public string TagNumber { get; private set; }
        
        public DateTime CheckIn { get; private set; }

        public DateTime CheckOut { get; private set; }

        public decimal ElaspedTime { get; set; }

        public decimal HourlyFee { get; private set; }

        public decimal Total { get; }

        public ParkOut(string tagNumber, DateTime checkIn, DateTime checkOut, decimal hourlyFee, decimal total)
        {
            InsertTime = DateTime.Now;
            TagNumber = tagNumber;
            CheckIn = checkIn;
            CheckOut = checkOut;
            HourlyFee = hourlyFee;

            TimeSpan diff = checkOut - checkIn;

            int hour = Convert.ToInt32(Math.Truncate(diff.TotalHours));
            decimal minute = 0;
            if (hour > 0)
            {
                minute = diff.Minutes;
            }
            else
            {
                minute = Math.Round(Convert.ToDecimal(diff.TotalMinutes));
            }

            ElaspedTime = Convert.ToDecimal(Math.Round(hour + minute / 100,2));

            Total = total;

        }
    }
}
