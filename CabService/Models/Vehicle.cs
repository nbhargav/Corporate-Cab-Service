using System.ComponentModel.DataAnnotations;

namespace CabService.Models
{
    public enum VehicleStatus
    {
        Available,
        Assigned,
        Maintenance
    }

    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required, StringLength(20)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Model { get; set; } = string.Empty;

        public int SeatingCapacity { get; set; }

        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        // Assigned driver, if any
        public int? DriverId { get; set; }
        public Employee? Driver { get; set; }

        public ICollection<VehicleRequest> Requests { get; set; } = new List<VehicleRequest>();
        public ICollection<FuelLog> FuelLogs { get; set; } = new List<FuelLog>();
        public ICollection<Feedback> FeedbackEntries { get; set; } = new List<Feedback>();
        public VehicleInsurance? Insurance { get; set; }
    }
}
