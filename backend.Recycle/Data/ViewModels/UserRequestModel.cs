using System.ComponentModel.DataAnnotations;
using backend.Recycle.Data.Enums;
using backend.Recycle.Data.Models;

namespace backend.Recycle.Data.ViewModels
{
    public class UserRequestModel
    {
        public int AvailabilityZoneId { get; set; }

        [Required]
        public string fullAddress { get; set; }

        [Required(ErrorMessage = "Choose your wastes type!")]
        public rubbishType rubbishType { get; set; }

        [Required(ErrorMessage = "Choose your wastes size")]
        public size size { get; set; }

        public string photoUrl { get; set; }
    }
}
