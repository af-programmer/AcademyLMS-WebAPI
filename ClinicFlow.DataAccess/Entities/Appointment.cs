using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.DataAccess.Entities;

public class Appointment
{
    public int PatientId { get; set; }

    public int TreatmentId { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    public Patient Patient { get; set; } = null!;

    public Treatment Treatment { get; set; } = null!;
}
