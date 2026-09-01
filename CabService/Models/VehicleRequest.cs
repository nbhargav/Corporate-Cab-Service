using System.ComponentModel.DataAnnotations;

namespace CabService.Models
{
    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed
    }

    /// <summary>
    /// An employee's request to use a vehicle. Admin reviews and either
    /// assigns a vehicle (creating an AssignedVehicle record) or rejects it.
    /// </summary>
    public class VehicleRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required, StringLength(200)]
        public string Purpose { get; set; } = string.Empty;

        [Required]
        public DateTime RequiredFrom { get; set; }

        [Required]
        public DateTime RequiredTo { get; set; }

        [Required, StringLength(200)]
        public string PickupLocation { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string DropLocation { get; set; } = string.Empty;

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public AssignedVehicle? Assignment { get; set; }
    }
}
