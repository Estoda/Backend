using MyTherapy.Domain.Common;
using MyTherapy.Domain.Enums;

namespace MyTherapy.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid PatientId { get; set; }
    public User Patient { get; set; } = null!; 
    
    public Guid TherapistId { get; set; }
    public Therapist Therapist { get; set; } = null!;

    public Guid SlotId { get; set; }
    public AvailabilitySlot Slot { get; set; } = null!;

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}
