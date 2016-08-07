using System;

namespace BaalPratibha.Logic
{
    public interface IViewHelper
    {
        string CurrentUrl { get; }
        string GetYoutubeEmbedSrc(Uri youtubeUri);
    }
}
