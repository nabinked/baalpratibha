using System.ComponentModel.DataAnnotations;
using BaalPratibha.Models.Enums;

namespace BaalPratibha.Models
{
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public Roles Role { get; set; }
    }
}
