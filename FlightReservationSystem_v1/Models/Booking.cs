using System;
using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem_v1.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Foreign Key for Flight
        public int FlightId { get; set; }
        public required Flight Flight { get; set; }

        // Foreign Key for User
        public int UserId { get; set; }

        [Required] // Optional: Use this attribute to make it mandatory
        public int PassengerCount { get; set; }
        public required User User { get; set; }
    }
}
