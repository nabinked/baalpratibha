using System;
using System.ComponentModel.DataAnnotations;

namespace BaalPratibha.Models
{
    public class ContestantDetail
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Suburb { get; set; }
        [Required]
        public string ParentsName { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string AboutMe { get; set; }

        public Uri PerformanceVideoUrl { get; set; }
    }
}
