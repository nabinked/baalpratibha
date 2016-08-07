namespace BaalPratibha.Models.ViewModels
{
    public class ProfileImageViewModel
    {
        public long UserId { get; set; }
        public string ImageCssClass { get; set; } = "img-rounded";
        public string Height { get; set; }
        public string Width { get; set; }
    }
}
