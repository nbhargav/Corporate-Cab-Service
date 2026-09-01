using System.ComponentModel.DataAnnotations;

namespace CabService.Models
{
    /// <summary>
    /// Entered by drivers to track fuel usage per vehicle, used for admin reporting.
    /// </summary>
    public class FuelLog
    {
        [Key]
        public int FuelLogId { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        public int DriverId { get; set; }
        public Employee? Driver { get; set; }

        [Required]
        public DateTime LogDate { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal FuelQuantityLiters { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public int OdometerReading { get; set; }

        [StringLength(300)]
        public string? Notes { get; set; }
    }
}
