using System.Text.Json;
using Portfolio.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Forms;

namespace Portfolio.Services;

public class ArtworkService
{
    private readonly IWebHostEnvironment _environment;

    public ArtworkService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    private string DataFolder => Path.Combine(_environment.WebRootPath, "data");
    private string JsonPath => Path.Combine(DataFolder, "artworks.json");

    public async Task<List<ArtworkItem>> GetArtworksAsync()
    {
        if (!File.Exists(JsonPath))
            return new List<ArtworkItem>();

        var json = await File.ReadAllTextAsync(JsonPath);

        return JsonSerializer.Deserialize<List<ArtworkItem>>(json) ?? new List<ArtworkItem>();
    }

    public async Task AddArtworkAsync(ArtworkItem artwork)
    {
        var artworks = await GetArtworksAsync();

        artworks.Add(artwork);

        Directory.CreateDirectory(DataFolder);

        var json = JsonSerializer.Serialize(artworks, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(JsonPath, json);
    }

    public async Task DeleteArtworkAsync(Guid id)
    {
        var artworks = await GetArtworksAsync();

        var item = artworks.FirstOrDefault(x => x.Id == id);

        if (item is not null)
        {
            artworks.Remove(item);

            var json = JsonSerializer.Serialize(artworks, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(JsonPath, json);
        }
    }

    public async Task<string> SaveImageAsync(IBrowserFile file)
    {
        var uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "gallery",
            "uploads"
        );

        Directory.CreateDirectory(uploadsFolder);

        var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
        var filePath = Path.Combine(uploadsFolder, safeFileName);

        await using var uploadStream =
            file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);

        await using var fileStream = File.Create(filePath);

        await uploadStream.CopyToAsync(fileStream);

        return $"/gallery/uploads/{safeFileName}";
    }
}