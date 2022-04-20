namespace backend.Recycle.Data.Models.Identity
{
    using System.ComponentModel.DataAnnotations;
    public class RegisterUserRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Address  { get; set; }
        [Required]
        public string SSID  { get; set; }
    }
}
