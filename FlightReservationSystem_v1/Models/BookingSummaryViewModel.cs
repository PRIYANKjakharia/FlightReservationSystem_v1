namespace FlightReservationSystem_v1.ViewModels
{
    public class BookingSummaryViewModel
    {
        public required string FlightNumber { get; set; }
        public string Airline { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public required string Source { get; set; }
        public required string Destination { get; set; }
        public decimal TotalPrice { get; set; }
        public int PassengerCount { get; set; }
        public DateTime BookingDate { get; set; }
        public int FlightId { get; set; }
        public int UserId { get; set; }
    }
}
