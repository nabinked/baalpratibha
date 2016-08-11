using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BaalPratibha.DbPortal;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BaalPratibha.Logic
{
    public class ViewHelper : IViewHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserDb _userDb;

        public ViewHelper(IHttpContextAccessor httpContextAccessor, UserDb userDb)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDb = userDb;
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


            return youtubeMatch.Success ? "https://www.youtube.com/embed/" + youtubeMatch.Groups[1].Value : "";

        }

        public async Task<long> GetShares(string userName)
        {
            using (var client = new HttpClient())
            {
                var user = _userDb.GetUserByUserName(userName);
                if (user != null)
                {
                    var host = _httpContextAccessor.HttpContext.Request.Scheme + "://" +
                               _httpContextAccessor.HttpContext.Request.Host;
                    var url = host + "/" + userName;
                    var baseUri = "http://graph.facebook.com/?id=" + url;
                    client.BaseAddress = new Uri(baseUri);
                    try
                    {
                        var response = await client.GetStringAsync(baseUri);
                        var json = JsonConvert.DeserializeObject<dynamic>(response);
                        return json.shares;
                    }
                    catch (Exception)
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
    }
}
