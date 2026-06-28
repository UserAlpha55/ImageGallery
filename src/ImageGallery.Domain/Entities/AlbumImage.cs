namespace ImageGallery.Domain.Entities;

public class AlbumImage
{
    public Guid AlbumId { get; set; }
    public Guid ImageId { get; set; }
    public int SortOrder { get; set; }

    public Album Album { get; set; } = null!;
    public Image Image { get; set; } = null!;
}
