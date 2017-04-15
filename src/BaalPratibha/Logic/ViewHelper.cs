using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BaalPratibha.DbPortal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Converters;

namespace BaalPratibha.Logic
{
    public class ViewHelper : IViewHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserDb _userDb;
        private AppSettings _appsettings;

        public ViewHelper(IHttpContextAccessor httpContextAccessor, UserDb userDb, IOptions<AppSettings> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDb = userDb;
            _appsettings = options.Value;
        }

        public string CurrentUrl => GetCurrentUrl();

        public HttpRequest Request => _httpContextAccessor.HttpContext.Request;

        private string GetCurrentUrl()
        {
            return Request.Scheme + "://" + Request.Host + Request.Path + Request.QueryString;
        }

        public string GetYoutubeEmbedSrc(string youtubeUri)
        {
            if (string.IsNullOrEmpty(youtubeUri))
            {
                return "";
            }
            const string youTubeRegexpattern = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";
            var youtubeVideoRegex = new Regex(youTubeRegexpattern);
            var youtubeMatch = youtubeVideoRegex.Match(youtubeUri);


            return youtubeMatch.Success ? "https://www.youtube.com/embed/" + youtubeMatch.Groups[1].Value + "?autoplay=1" : "";

        }

        public async Task<long> GetShares(string userName)
        {
            using (var client = new HttpClient())
            {
                var user = _userDb.GetUserByUserName(userName);
                if (user != null)
                {
                    var host = "http://www.baalpratibha.com.au";
                    var url = host + "/" + userName;
                    var baseUri = "http://graph.facebook.com/?id=" + url;
                    client.BaseAddress = new Uri(baseUri);
                    try
                    {
                        var response = await client.GetStringAsync(baseUri);
                        var json = JsonConvert.DeserializeObject<dynamic>(response);
                        dynamic share = json.share;
                        return share.share_count;
                    }
                    catch (Exception ex)
                    {

                        return 0;
                    }



                }
                else
                {
                    return 0;
                }
            }

        }

        public string GetAnnouncement()
        {
            var announcement = "";
            var systemTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            if (_appsettings.IsVotingPeriod)
            {
                announcement = "Voting is now open. Voting ends on " + TimeZoneInfo.ConvertTime(_appsettings.VotingEndDateUtc, systemTimeZone).ToString("f");
            }
            else
            {
                if (DateTime.UtcNow < _appsettings.VotingStartDateUtc)
                {
                    announcement = "Voting will start on " + TimeZoneInfo.ConvertTime(_appsettings.VotingStartDateUtc, systemTimeZone).ToString("f");
                }
                if (DateTime.UtcNow > _appsettings.VotingEndDateUtc)
                {
                    announcement = "Voting has ended on " + TimeZoneInfo.ConvertTime(_appsettings.VotingEndDateUtc, systemTimeZone).ToString("f");
                }

            }

            return announcement;
        }

        public bool IsVotingPeriod => _appsettings.IsVotingPeriod;

        public string GetOrderBy(bool showVoteDetails, string orderBy)
        {

            if (!showVoteDetails)
            {
                if (orderBy.ToLower().Contains("rank"))
                {
                    return "fullname_asc";
                }
                else
                {
                    return string.IsNullOrWhiteSpace(orderBy.Trim()) ? "fullname_asc" : orderBy;
                }

            }
            return string.IsNullOrWhiteSpace(orderBy.Trim()) ? "rank_asc" : orderBy;

        }

        public bool ShowVoteDetails(bool isAdmin, bool showVotes)
        {
            if (showVotes)
            {
                return true;
            }
            else
            {
                if (isAdmin)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
