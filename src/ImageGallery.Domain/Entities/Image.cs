namespace ImageGallery.Domain.Entities;

public class Image
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public long SizeBytes { get; set; }
    public string? GitHubRepoUrl { get; set; }
    public string? GitHubPath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AlbumImage> AlbumImages { get; set; } = new List<AlbumImage>();
}
