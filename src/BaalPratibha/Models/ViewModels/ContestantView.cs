using System;

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
        public string Suburb { get; set; } = "Not Provided";
        public string ParentsName { get; set; } = "Not Provided";
        public string Contact { get; set; } = "Not Provided";
        public string Email { get; set; } = "Not Provided";
        public string AboutMe { get; set; } = "I am a contestant in Baal Pratibha";
        public int TotalVotes { get; set; }
        public Uri PerformanceVideoUrl { get; set; } = new Uri("https://www.youtube.com/watch?v=Fb2LE3VRB7g");
    }
}
