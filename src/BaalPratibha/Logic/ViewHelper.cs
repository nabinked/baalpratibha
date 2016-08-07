using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace BaalPratibha.Logic
{
    public class ViewHelper : IViewHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUrl => GetCurrentUrl();

        public HttpRequest Request => _httpContextAccessor.HttpContext.Request;

        private string GetCurrentUrl()
        {
            return Request.Scheme + "://" + Request.Host + Request.Path + Request.QueryString;
        }

        public string GetYoutubeEmbedSrc(Uri youtubeUri)
        {
            const string youTubeRegexpattern = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";
            var youtubeVideoRegex = new Regex(youTubeRegexpattern);
            var youtubeMatch = youtubeVideoRegex.Match(youtubeUri.ToString());


            return youtubeMatch.Success ? "https://www.youtube.com/embed/" + youtubeMatch.Groups[1].Value : "";

        }
    }
}
