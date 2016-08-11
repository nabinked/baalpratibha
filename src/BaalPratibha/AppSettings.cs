using System;

namespace BaalPratibha
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public Uri DefaultImageUrl { get; set; }
        public string UserImagesFolder { get; set; }
        public string NotFoundImagePath { get; set; }
    }
}
