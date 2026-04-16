namespace Portfolio.Models
{
    public class ArtworkFormModel
    {
        public string Title { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Flow { get; set; } = string.Empty;
    }
}