namespace Portfolio.Models
{
    public class PortfolioItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Idea { get; set; } = string.Empty;
        public string Structure { get; set; } = string.Empty;
        public string DemoUrl { get; set; } = string.Empty;
        public string GitHubUrl { get; set; } = string.Empty;
    }
}
