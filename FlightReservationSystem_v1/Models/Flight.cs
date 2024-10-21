using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FlightReservationSystem_v1.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        public required string FlightNumber { get; set; }

        public required string Airline { get; set; }

        public required string DepartureCity { get; set; }

       public required string ArrivalCity { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Price { get; set; }


    // Navigation property: A flight can have many bookings
    public required ICollection<Booking> Bookings { get; set; }
    }
}
