using System;

namespace BaalPratibha
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public Uri DefaultImageUrl { get; set; }
        public string UserImagesFolder { get; set; }
        public string NotFoundImagePath { get; set; }
        public DateTime VotingStartDateUtc { get; set; } = new DateTime(2016, 09, 10, 14, 0, 0, DateTimeKind.Utc);
        public DateTime VotingEndDateUtc { get; set; } = new DateTime(2016, 09, 17, 14, 0, 0, DateTimeKind.Utc);
        public bool ShowVotes { get; set; }
        public string Announcement{ get; set; }

        public bool IsVotingPeriod => DateTime.UtcNow > VotingStartDateUtc && DateTime.UtcNow < VotingEndDateUtc;
    }
}
