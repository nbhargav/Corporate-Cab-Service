using System.ComponentModel.DataAnnotations;

namespace CabService.Models
{
    /// <summary>
    /// Created once an admin approves a VehicleRequest and assigns a specific
    /// vehicle/driver to it.
    /// </summary>
    public class AssignedVehicle
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        public int RequestId { get; set; }
        public VehicleRequest? Request { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public int? DriverId { get; set; }
        public Employee? Driver { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        [Required, StringLength(100)]
        public string AssignedByAdminUsername { get; set; } = string.Empty;
    }
}
