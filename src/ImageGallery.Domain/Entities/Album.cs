namespace ImageGallery.Domain.Entities;

public class Album
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AlbumImage> AlbumImages { get; set; } = new List<AlbumImage>();
}
