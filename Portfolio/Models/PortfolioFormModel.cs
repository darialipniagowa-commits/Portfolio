namespace Portfolio.Models
{
    public class PortfolioFormModel
    {
        public string Name { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Idea { get; set; } = string.Empty;
        public string Structure { get; set; } = string.Empty;
        public string? DemoUrl { get; set; }
        public string? GitHubUrl { get; set; }
    }
}
