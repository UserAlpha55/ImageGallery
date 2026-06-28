using Microsoft.EntityFrameworkCore;
using ImageGallery.Domain.Entities;
using ImageGallery.Infrastructure.Data;
using ImageGallery.Contracts;

namespace ImageGallery.Application.Services;

public interface IImageService
{
    Task<List<ImageDto>> GetAllAsync(string? search, string? sort, Guid? albumId);
    Task<ImageDto?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<List<ImageDto>> BulkImportAsync(List<string> urls);
}

public class ImageService : IImageService
{
    private readonly AppDbContext _db;

    public ImageService(AppDbContext db) => _db = db;

    public async Task<List<ImageDto>> GetAllAsync(string? search, string? sort, Guid? albumId)
    {
        var query = _db.Images.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(i => i.FileName.Contains(search) || (i.Description != null && i.Description.Contains(search)));

        if (albumId.HasValue)
            query = query.Where(i => i.AlbumImages.Any(ai => ai.AlbumId == albumId.Value));

        query = sort?.ToLower() switch
        {
            "name" => query.OrderBy(i => i.FileName),
            "name_desc" => query.OrderByDescending(i => i.FileName),
            "oldest" => query.OrderBy(i => i.CreatedAt),
            "size" => query.OrderByDescending(i => i.SizeBytes),
            "size_asc" => query.OrderBy(i => i.SizeBytes),
            _ => query.OrderByDescending(i => i.CreatedAt)
        };

        return await query.Select(i => new ImageDto
        {
            Id = i.Id,
            FileName = i.FileName,
            Url = i.Url,
            Description = i.Description,
            Width = i.Width,
            Height = i.Height,
            SizeBytes = i.SizeBytes,
            GitHubRepoUrl = i.GitHubRepoUrl,
            CreatedAt = i.CreatedAt,
            AlbumNames = i.AlbumImages.Select(ai => ai.Album.Name).ToList()
        }).ToListAsync();
    }

    public async Task<ImageDto?> GetByIdAsync(Guid id)
    {
        return await _db.Images
            .Where(i => i.Id == id)
            .Select(i => new ImageDto
            {
                Id = i.Id,
                FileName = i.FileName,
                Url = i.Url,
                Description = i.Description,
                Width = i.Width,
                Height = i.Height,
                SizeBytes = i.SizeBytes,
                GitHubRepoUrl = i.GitHubRepoUrl,
                CreatedAt = i.CreatedAt,
                AlbumNames = i.AlbumImages.Select(ai => ai.Album.Name).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var image = await _db.Images.FindAsync(id);
        if (image is null) return;
        _db.Images.Remove(image);
        await _db.SaveChangesAsync();
    }

    public async Task<List<ImageDto>> BulkImportAsync(List<string> urls)
    {
        var images = urls.Select(url => new Image
        {
            Id = Guid.NewGuid(),
            Url = url,
            FileName = Path.GetFileName(new Uri(url).AbsolutePath),
            SizeBytes = 0,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        _db.Images.AddRange(images);
        await _db.SaveChangesAsync();

        return images.Select(i => new ImageDto
        {
            Id = i.Id,
            FileName = i.FileName,
            Url = i.Url,
            SizeBytes = i.SizeBytes,
            CreatedAt = i.CreatedAt
        }).ToList();
    }
}
