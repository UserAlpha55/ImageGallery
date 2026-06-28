namespace ImageGallery.Contracts;

public class ImageDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public long SizeBytes { get; set; }
    public string? GitHubRepoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> AlbumNames { get; set; } = new();
}
