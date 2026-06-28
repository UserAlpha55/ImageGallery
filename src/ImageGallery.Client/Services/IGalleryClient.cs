using ImageGallery.Contracts;

namespace ImageGallery.Client.Services;

public interface IGalleryClient
{
    Task<List<ImageDto>> GetImagesAsync(string? search = null, string? sort = null, Guid? albumId = null);
    Task<ImageDto?> GetImageAsync(Guid id);
    Task DeleteImageAsync(Guid id);
    Task<List<ImageDto>> BulkImportAsync(List<string> urls);

    Task<List<AlbumDto>> GetAlbumsAsync();
    Task<AlbumDetailDto?> GetAlbumAsync(Guid id);
    Task<AlbumDto> CreateAlbumAsync(CreateAlbumRequest request);
    Task UpdateAlbumAsync(Guid id, UpdateAlbumRequest request);
    Task DeleteAlbumAsync(Guid id);
    Task AddImageToAlbumAsync(Guid albumId, Guid imageId);
    Task RemoveImageFromAlbumAsync(Guid albumId, Guid imageId);
    Task ReorderAlbumImagesAsync(Guid albumId, List<ReorderItem> items);

    Task<List<GitHubFileDto>> PreviewGitHubRepoAsync(ImportRequest request);
}
