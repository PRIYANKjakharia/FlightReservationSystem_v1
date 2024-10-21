using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservationSystem_v1.Data;
using FlightReservationSystem_v1.Models;

namespace FlightReservationSystem_v1.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Flight)  // Include related Flight data
                .Include(b => b.User);   // Include related User data
            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/BookFlight
        public async Task<IActionResult> BookFlight(int flightId)
        {
            ViewBag.FlightId = flightId;

            var users = await _context.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "UserId", "FirstName"); // Display full names if required

            return View();
        }

        // POST: Bookings/BookFlight
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookFlight(int flightId, [Bind("TotalPrice, UserId, PassengerCount")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var flight = await _context.Flights.FindAsync(flightId);
                if (flight == null)
                {
                    return NotFound();
                }

                booking.FlightId = flightId;
                booking.BookingDate = DateTime.Now;
                booking.TotalPrice = flight.Price * booking.PassengerCount;

                _context.Add(booking);
                await _context.SaveChangesAsync();

                // Redirect to Summary view (you can modify the parameters as needed)
                return RedirectToAction("Summary", new { bookingId = booking.BookingId });
            }

            ViewBag.FlightId = flightId;
            var users = await _context.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "UserId", "FirstName");

            return View(booking);
        }
        // GET: Bookings/Summary
        public async Task<IActionResult> Summary(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking); // Ensure you have a Summary view set up
        }




        //// Confirm Booking
        //public async Task<IActionResult> ConfirmBooking(int bookingId)
        //{
        //    var booking = await _context.Bookings
        //        .Include(b => b.Flight)
        //        .Include(b => b.User)
        //        .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }

        //    // Prepare summary data
        //    var bookingSummary = new
        //    {
        //        booking.Flight.DepartureCity,
        //        booking.Flight.ArrivalCity,
        //        booking.TotalPrice,
        //        booking.BookingDate,
        //        booking.PassengerCount
        //    };

        //    return View(bookingSummary); // Create a new view for the booking summary
        //}

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", booking.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", booking.UserId); // Modify as per your requirements

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId, BookingDate, TotalPrice, FlightId, UserId, PassengerCount")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", booking.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "FirstName", booking.UserId); // Modify as needed

            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
