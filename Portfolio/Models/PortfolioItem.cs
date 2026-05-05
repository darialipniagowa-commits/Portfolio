namespace Portfolio.Models
{
    public class PortfolioItem
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? Year { get; set; }
        public string? ImageUrl { get; set; }
        public string? DemoUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? Idea { get; set; }
        public string? Structure { get; set; }
    }
}
