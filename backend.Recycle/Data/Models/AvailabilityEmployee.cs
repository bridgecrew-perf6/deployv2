namespace backend.Recycle.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AvailabilityEmployee
    {
        [Key]
        public int Id { get; set; }
        public AvailabilityZone AvailabilityZone { get; set; }

        [Required]
        public int AvailabilityZoneId { get; set; }

        public Users Employee { get; set; }

        [Required]
        public string EmployeeId { get; set; }

    }
}
