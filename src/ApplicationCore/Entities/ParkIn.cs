namespace ApplicationCore.Entities
{
    public class ParkIn : BaseEntity
    {
        public string TagNumber { get; private set; }
        public DateTime CheckIn { get; private set; }

        public ParkIn(string tagNumber, DateTime checkIn)
        {
            TagNumber = tagNumber;
            CheckIn = checkIn;
        }
    }
}
