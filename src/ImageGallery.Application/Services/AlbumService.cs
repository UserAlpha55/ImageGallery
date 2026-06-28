using Microsoft.EntityFrameworkCore;
using ImageGallery.Domain.Entities;
using ImageGallery.Infrastructure.Data;
using ImageGallery.Contracts;

namespace ImageGallery.Application.Services;

public interface IAlbumService
{
    Task<List<AlbumDto>> GetAllAsync();
    Task<AlbumDetailDto?> GetByIdAsync(Guid id);
    Task<AlbumDto> CreateAsync(CreateAlbumRequest request);
    Task UpdateAsync(Guid id, UpdateAlbumRequest request);
    Task DeleteAsync(Guid id);
    Task AddImageAsync(Guid albumId, Guid imageId);
    Task RemoveImageAsync(Guid albumId, Guid imageId);
    Task ReorderImagesAsync(Guid albumId, List<ReorderItem> items);
}

public class AlbumService : IAlbumService
{
    private readonly AppDbContext _db;

    public AlbumService(AppDbContext db) => _db = db;

    public async Task<List<AlbumDto>> GetAllAsync()
    {
        return await _db.Albums
            .Select(a => new AlbumDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                CoverImageUrl = a.CoverImageUrl,
                CreatedAt = a.CreatedAt,
                ImageCount = a.AlbumImages.Count
            })
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<AlbumDetailDto?> GetByIdAsync(Guid id)
    {
        return await _db.Albums
            .Where(a => a.Id == id)
            .Select(a => new AlbumDetailDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                CoverImageUrl = a.CoverImageUrl,
                CreatedAt = a.CreatedAt,
                Images = a.AlbumImages
                    .OrderBy(ai => ai.SortOrder)
                    .Select(ai => new AlbumImageDto
                    {
                        ImageId = ai.ImageId,
                        FileName = ai.Image.FileName,
                        Url = ai.Image.Url,
                        SortOrder = ai.SortOrder
                    }).ToList()
            }).FirstOrDefaultAsync();
    }

    public async Task<AlbumDto> CreateAsync(CreateAlbumRequest request)
    {
        var album = new Album
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        _db.Albums.Add(album);
        await _db.SaveChangesAsync();

        return new AlbumDto
        {
            Id = album.Id,
            Name = album.Name,
            Description = album.Description,
            CreatedAt = album.CreatedAt,
            ImageCount = 0
        };
    }

    public async Task UpdateAsync(Guid id, UpdateAlbumRequest request)
    {
        var album = await _db.Albums.FindAsync(id);
        if (album is null) return;

        if (request.Name is not null) album.Name = request.Name;
        if (request.Description is not null) album.Description = request.Description;
        if (request.CoverImageUrl is not null) album.CoverImageUrl = request.CoverImageUrl;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var album = await _db.Albums.FindAsync(id);
        if (album is null) return;
        _db.Albums.Remove(album);
        await _db.SaveChangesAsync();
    }

    public async Task AddImageAsync(Guid albumId, Guid imageId)
    {
        if (await _db.AlbumImages.AnyAsync(ai => ai.AlbumId == albumId && ai.ImageId == imageId))
            return;

        var maxOrder = await _db.AlbumImages
            .Where(ai => ai.AlbumId == albumId)
            .MaxAsync(ai => (int?)ai.SortOrder) ?? 0;

        _db.AlbumImages.Add(new AlbumImage
        {
            AlbumId = albumId,
            ImageId = imageId,
            SortOrder = maxOrder + 1
        });

        await _db.SaveChangesAsync();
    }

    public async Task RemoveImageAsync(Guid albumId, Guid imageId)
    {
        var link = await _db.AlbumImages
            .FirstOrDefaultAsync(ai => ai.AlbumId == albumId && ai.ImageId == imageId);
        if (link is null) return;
        _db.AlbumImages.Remove(link);
        await _db.SaveChangesAsync();
    }

    public async Task ReorderImagesAsync(Guid albumId, List<ReorderItem> items)
    {
        var albumImages = await _db.AlbumImages
            .Where(ai => ai.AlbumId == albumId)
            .ToListAsync();

        foreach (var item in items)
        {
            var ai = albumImages.FirstOrDefault(x => x.ImageId == item.ImageId);
            if (ai is not null)
                ai.SortOrder = item.SortOrder;
        }

        await _db.SaveChangesAsync();
    }
}
