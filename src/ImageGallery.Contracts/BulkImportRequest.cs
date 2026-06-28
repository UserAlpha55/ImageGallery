namespace ImageGallery.Contracts;

public class BulkImportRequest
{
    public List<string> ImageUrls { get; set; } = new();
}
