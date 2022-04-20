namespace backend.Recycle.Data.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using backend.Recycle.Data.Enums;
    public class editRequest
    {
        [Required(ErrorMessage = "Choose your wastes type!")]
        public rubbishType rubbishType { get; set; }

        [Required(ErrorMessage = "Choose your wastes size")]
        public size size { get; set; }

        public string photoUrl { get; set; }
    }
}
