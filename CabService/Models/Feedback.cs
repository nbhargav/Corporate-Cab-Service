using System.ComponentModel.DataAnnotations;

namespace CabService.Models
{
    /// <summary>
    /// Employee feedback on a driver/vehicle after a trip, reviewable by admin.
    /// </summary>
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
