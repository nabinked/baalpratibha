using System.ComponentModel.DataAnnotations;

namespace BaalPratibha.Models
{
    public class Vote
    {
        public int Id { get; set; }
        [Required]
        public string VoterName { get; set; }
        [Required]
        [EmailAddress]
        public string VoterEmail { get; set; }
        [Required]
        public int ContestantId { get; set; }
    }
}
