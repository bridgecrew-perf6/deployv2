using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Recycle.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    
    public class Users : IdentityUser
    {
        [Required]
        public string SSID  { get; set; }
        public string Address { get; set; }
    }
}
