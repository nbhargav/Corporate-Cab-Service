using System.ComponentModel.DataAnnotations;

namespace CabService.Models
{
    public enum UserRole
    {
        Admin,
        Employee,
        Driver
    }

    /// <summary>
    /// Represents any system user: Admin, Employee, or Driver.
    /// Role determines which interface/permissions apply (see AccountController).
    /// </summary>
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<VehicleRequest> VehicleRequests { get; set; } = new List<VehicleRequest>();
        public ICollection<Feedback> FeedbackEntries { get; set; } = new List<Feedback>();
    }
}
