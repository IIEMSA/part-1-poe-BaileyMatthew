using EventBooking.Data;
using EventBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace EventBooking.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.EventId = new SelectList(await _context.Event.ToListAsync(), "EventId", "EventName");
            ViewBag.VenueId = new SelectList(await _context.Venue.ToListAsync(), "VenueId", "VenueName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventId = new SelectList(await _context.Event.ToListAsync(), "EventId", "EventName", booking.EventId);
            ViewBag.VenueId = new SelectList(await _context.Venue.ToListAsync(), "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // ================================
        // Edit Functionality Starts Here
        // ================================

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
                return NotFound();

            ViewBag.EventId = new SelectList(await _context.Event.ToListAsync(), "EventId", "EventName", booking.EventId);
            ViewBag.VenueId = new SelectList(await _context.Venue.ToListAsync(), "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
                return NotFound();

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
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventId = new SelectList(await _context.Event.ToListAsync(), "EventId", "EventName", booking.EventId);
            ViewBag.VenueId = new SelectList(await _context.Venue.ToListAsync(), "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }

        // ================================

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
