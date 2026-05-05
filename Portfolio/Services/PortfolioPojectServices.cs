using Portfolio.Models;

namespace Portfolio.Services;

public class PortfolioProjectService
{
    public List<PortfolioItem> Projects { get; } = new();

    public void Add(PortfolioItem item)
    {
        Projects.Add(item);
    }

    public void Delete(Guid id)
    {
        var item = Projects.FirstOrDefault(x => x.Id == id);

        if (item is not null)
            Projects.Remove(item);
    }
}