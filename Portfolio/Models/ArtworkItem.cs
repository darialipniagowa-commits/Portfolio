public class ArtworkItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? Description { get; set; }
    public string Flow { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal? Price { get; set; }
}