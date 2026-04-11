using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MyTherapy.Domain.Enums;
using MyTherapy.Infrastructure.Persistence;
using MyTherapy.Application.DTOs.Slots;
using System.Security.Claims;
using MyTherapy.Domain.Entities;
using MyTherapy.Domain.Enums;

namespace MyTherapy.API.Controllers;

[Authorize(Roles = "Patient")]
[ApiController]
[Route("api/patient/bookings")]
public class PatientBookingController : ControllerBase
{
    private readonly AppDbContext _context;

    public PatientBookingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(CreateBookingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var patientId = Guid.Parse(userId); // Convert string to Guid
        var patient = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == patientId && u.Role == Role.Patient);

        var slot = await _context.AvailabilitySlots
            .Include(s => s.Therapist)
            .FirstOrDefaultAsync(s => s.Id == request.SlotId);

        if (slot == null)
            return NotFound("Slot not found");

        if (slot.IsBooked)
            return BadRequest("Slot already booked");

        if (patient == null)
            return NotFound("Patient not found");

        var booking = new Booking
        {
            PatientId = patient.Id,
            TherapistId = slot.TherapistId,
            SlotId = slot.Id,
            Status = BookingStatus.Confirmed
        };

        slot.IsBooked = true;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return Ok(booking);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var patientId = Guid.Parse(userId); // Convert string to Guid
        var patient = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == patientId && u.Role == Role.Patient);

        if (patient == null)
            return NotFound("Patient not found.");

        var bookings = await _context.Bookings
            .Include(b => b.Slot)
            .Include(b => b.Therapist)
            .Where(b => b.PatientId == patient.Id)
            .ToListAsync();

        return Ok(bookings);
    }
}
