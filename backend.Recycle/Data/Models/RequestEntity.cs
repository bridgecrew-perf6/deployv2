namespace backend.Recycle.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using backend.Recycle.Data.Enums;

    public class RequestEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AvailabilityZoneId { get; set; }
        public AvailabilityZone AvailabilityZone { get; set; }

        [Required]
        public string fullAddress { get; set; }

        [Required(ErrorMessage = "Choose your wastes type!")]
        public rubbishType rubbishType { get; set; }

        [Required(ErrorMessage = "Choose your wastes size")]
        public size size { get; set; }

        public string photoUrl { get; set; }

        public string UserId { get; set; }
        public Users  User { get; set; }

        public string EmployeeId { get; set; }
        public Users Employee { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime requestDateTime { get; set; } 

    }
}
