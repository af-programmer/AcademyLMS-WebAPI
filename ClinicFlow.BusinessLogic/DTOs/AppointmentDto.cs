using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.BusinessLogic.DTOs;

public class AppointmentDto
{
    [Required(ErrorMessage = "Patient id is required.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Treatment id is required.")]
    public int TreatmentId { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [Range(0, 3, ErrorMessage = "Status must be between 0 (Scheduled) and 3 (No-show).")]
    public int Status { get; set; }

    [Required(ErrorMessage = "Appointment date is required.")]
    public DateTime AppointmentDate { get; set; }
}
