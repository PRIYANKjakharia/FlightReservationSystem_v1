using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightReservationSystem_v1.Data;
using FlightReservationSystem_v1.Models;

namespace FlightReservationSystem_v1.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights.ToListAsync());
        }
        // GET: Flights/AllFlights
        public async Task<IActionResult> AllFlights()
        {
            var flights = await _context.Flights.ToListAsync();
            return View(flights); // Make sure you have a view named AllFlights.cshtml
        }


        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightId,FlightNumber,Airline,DepartureCity,ArrivalCity,DepartureTime,ArrivalTime,Price")] Flight flight)
        {
            _context.Add(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,FlightNumber,Airline,DepartureCity,ArrivalCity,DepartureTime,ArrivalTime,Price")] Flight flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(flight);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(flight.FlightId))
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

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }

        // GET: Flights/Search
        public IActionResult Search()
        {
            return View();
        }

        // GET: Flights/Search
        [HttpGet]
        public async Task<IActionResult> Search(string source, string destination, DateTime? date)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination) || !date.HasValue || date.Value < DateTime.Today)
            {
                ModelState.AddModelError(string.Empty, "Please enter valid search criteria.");
                return View(); // Make sure this returns the correct view
            }

            var flights = await _context.Flights
                .Where(f => f.DepartureCity.ToLower() == source.ToLower() &&
                            f.ArrivalCity.ToLower() == destination.ToLower() &&
                            f.DepartureTime.Date == date.Value.Date)
                .ToListAsync();

            if (flights.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No flights available for the selected criteria.");
            }

            return View("SearchResults", flights);
        }



        // POST: Flights/SearchResults
        [HttpPost]
        public async Task<IActionResult> Search(string source, string destination, DateTime date)
        {
            // Ensure the date is upcoming
            if (date.Date < DateTime.Today)
            {
                // Handle invalid date case
                return BadRequest("Please select an upcoming date.");
            }

            // Search for flights
            var flights = await _context.Flights
                .Where(f => f.DepartureCity.ToLower() == source.ToLower() &&
                            f.ArrivalCity.ToLower() == destination.ToLower() &&
                            f.DepartureTime.Date == date.Date)
                .ToListAsync();

            return View(flights);
        }



        // POST: Flights/BookFlight
        [HttpPost]
        public IActionResult BookFlight(int flightId)
        {
            // Implement your booking logic here
            return RedirectToAction(nameof(Index)); // Redirect to the index or any other appropriate action
        }
    }
}
