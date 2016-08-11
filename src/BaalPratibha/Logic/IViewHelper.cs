using System;
using System.Threading.Tasks;

namespace BaalPratibha.Logic
{
    public interface IViewHelper
    {
        string CurrentUrl { get; }
        string GetYoutubeEmbedSrc(string youtubeUri);
        Task<long> GetShares(string userName);
    }
}
