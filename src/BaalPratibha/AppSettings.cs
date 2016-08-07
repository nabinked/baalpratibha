using System;

namespace BaalPratibha
{
    public class AppSettings
    {
        public string ConnectionStringDev { get; set; }
        public string ConnectionStringProd { get; set; }
        public Uri DefaultImageUrl { get; set; }
        public string UserImagesFolder { get; set; }
        public string NotFoundImagePath { get; set; }
    }
}
