namespace ImageGallery.Web.App.Services;

public class PageState
{
    public string? ImportRepoUrl { get; set; }
    public string? ImportRepoPath { get; set; }
    public string? GallerySearch { get; set; }
    public string GallerySort { get; set; } = "newest";
    public string? GalleryAlbum { get; set; }
    public HashSet<Guid> SelectedImageIds { get; set; } = new();
}
