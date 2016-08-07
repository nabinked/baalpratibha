using BaalPratibha.Models.Enums;

namespace BaalPratibha.Models.ViewModels
{
    public class UserView
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Roles Role { get; set; }
    }
}
