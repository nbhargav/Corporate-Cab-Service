using System.ComponentModel.DataAnnotations;

namespace CabService.Models
{
    public class VehicleInsurance
    {
        [Key]
        public int InsuranceId { get; set; }

        [Required]
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required, StringLength(50)]
        public string PolicyNumber { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Provider { get; set; } = string.Empty;

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public bool IsExpired => DateTime.UtcNow > ValidTo;
    }
}
