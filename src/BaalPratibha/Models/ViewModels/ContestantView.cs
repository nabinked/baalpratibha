using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BaalPratibha.Models.ViewModels
{
    public class ContestantView
    {

        public long Id { get; set; }
        public bool Selected { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; } = "Not Provided";
        public int Age { get; set; }
        public string Suburb { get; set; } = "Sydney";
        public string ParentsName { get; set; } = "Parent's Name";
        public string Contact { get; set; } = "Not Provided";
        public string Email { get; set; } = "Not Provided";
        public string AboutMe { get; set; } = "I am a contestant in Baal Pratibha. Please vote for me and share as much as you can.";
        public int TotalVotes { get; set; }

        public int TotalShares { get; set; }

        public int Rank { get; set; }
        public string PerformanceVideoUrl { get; set; }
    }
}
