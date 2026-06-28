using System.Net.Http.Json;
using ImageGallery.Contracts;

namespace ImageGallery.Client.Services;

public class GalleryClient : IGalleryClient
{
    private readonly HttpClient _http;

    public GalleryClient(HttpClient http) => _http = http;

    public async Task<List<ImageDto>> GetImagesAsync(string? search = null, string? sort = null, Guid? albumId = null)
    {
        var query = new Dictionary<string, string?>();
        if (search is not null) query["search"] = search;
        if (sort is not null) query["sort"] = sort;
        if (albumId is not null) query["albumId"] = albumId.ToString();

        var qs = string.Join("&", query.Where(kv => kv.Value is not null)
            .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value!)}"));

        return await _http.GetFromJsonAsync<List<ImageDto>>($"api/images?{qs}") ?? [];
    }

    public async Task<ImageDto?> GetImageAsync(Guid id)
        => await _http.GetFromJsonAsync<ImageDto>($"api/images/{id}");

    public async Task DeleteImageAsync(Guid id)
        => await _http.DeleteAsync($"api/images/{id}");

    public async Task<List<ImageDto>> BulkImportAsync(List<string> urls)
    {
        var response = await _http.PostAsJsonAsync("api/images/bulk-import", new BulkImportRequest { ImageUrls = urls });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ImageDto>>() ?? [];
    }

    public async Task<List<AlbumDto>> GetAlbumsAsync()
        => await _http.GetFromJsonAsync<List<AlbumDto>>("api/albums") ?? [];

    public async Task<AlbumDetailDto?> GetAlbumAsync(Guid id)
        => await _http.GetFromJsonAsync<AlbumDetailDto>($"api/albums/{id}");

    public async Task<AlbumDto> CreateAlbumAsync(CreateAlbumRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/albums", request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AlbumDto>())!;
    }

    public async Task UpdateAlbumAsync(Guid id, UpdateAlbumRequest request)
        => await _http.PutAsJsonAsync($"api/albums/{id}", request);

    public async Task DeleteAlbumAsync(Guid id)
        => await _http.DeleteAsync($"api/albums/{id}");

    public async Task AddImageToAlbumAsync(Guid albumId, Guid imageId)
        => await _http.PostAsync($"api/albums/{albumId}/images/{imageId}", null);

    public async Task RemoveImageFromAlbumAsync(Guid albumId, Guid imageId)
        => await _http.DeleteAsync($"api/albums/{albumId}/images/{imageId}");

    public async Task ReorderAlbumImagesAsync(Guid albumId, List<ReorderItem> items)
        => await _http.PutAsJsonAsync($"api/albums/{albumId}/images/reorder", new ReorderRequest { Items = items });

    public async Task<List<GitHubFileDto>> PreviewGitHubRepoAsync(ImportRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/github/preview", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<GitHubFileDto>>() ?? [];
    }
}
