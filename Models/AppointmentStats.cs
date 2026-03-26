namespace vita_care.Models
{
    public class AppointmentStats
    {
        public long Total { get; set; }
        public long Requested { get; set; }
        public long Approved { get; set; }
        public long Canceled { get; set; }
        public long Visited { get; set; }
        public long NotVisited { get; set; }
    }
}
