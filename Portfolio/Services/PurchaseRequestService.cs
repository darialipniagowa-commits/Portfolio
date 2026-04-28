using System.Text.Json;
using Portfolio.Models;

namespace Portfolio.Services;

public class PurchaseRequestService
{
    private readonly IWebHostEnvironment _environment;

    public PurchaseRequestService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    private string DataFolder => Path.Combine(_environment.WebRootPath, "data");
    private string JsonPath => Path.Combine(DataFolder, "purchase-requests.json");

    public async Task<List<PurchaseRequest>> GetRequestsAsync()
    {
        if (!File.Exists(JsonPath))
            return new List<PurchaseRequest>();

        var json = await File.ReadAllTextAsync(JsonPath);

        return JsonSerializer.Deserialize<List<PurchaseRequest>>(json)
               ?? new List<PurchaseRequest>();
    }

    public async Task AddRequestAsync(PurchaseRequest request)
    {
        Directory.CreateDirectory(DataFolder);

        var requests = await GetRequestsAsync();

        request.Id = Guid.NewGuid();
        request.CreatedAt = DateTime.Now;

        requests.Add(request);

        var json = JsonSerializer.Serialize(requests, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(JsonPath, json);
    }
}