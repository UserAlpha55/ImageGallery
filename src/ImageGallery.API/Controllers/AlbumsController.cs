using Microsoft.AspNetCore.Mvc;
using ImageGallery.Application.Services;
using ImageGallery.Contracts;

namespace ImageGallery.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumsController(IAlbumService albumService) => _albumService = albumService;

    [HttpGet]
    public async Task<ActionResult<List<AlbumDto>>> GetAll()
    {
        return await _albumService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlbumDetailDto>> GetById(Guid id)
    {
        var album = await _albumService.GetByIdAsync(id);
        if (album is null) return NotFound();
        return album;
    }

    [HttpPost]
    public async Task<ActionResult<AlbumDto>> Create([FromBody] CreateAlbumRequest request)
    {
        var album = await _albumService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = album.Id }, album);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAlbumRequest request)
    {
        await _albumService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _albumService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/images")]
    public async Task<ActionResult<List<AlbumImageDto>>> GetImages(Guid id)
    {
        var album = await _albumService.GetByIdAsync(id);
        if (album is null) return NotFound();
        return album.Images;
    }

    [HttpPost("{albumId}/images/{imageId}")]
    public async Task<IActionResult> AddImage(Guid albumId, Guid imageId)
    {
        await _albumService.AddImageAsync(albumId, imageId);
        return NoContent();
    }

    [HttpDelete("{albumId}/images/{imageId}")]
    public async Task<IActionResult> RemoveImage(Guid albumId, Guid imageId)
    {
        await _albumService.RemoveImageAsync(albumId, imageId);
        return NoContent();
    }

    [HttpPut("{albumId}/images/reorder")]
    public async Task<IActionResult> ReorderImages(Guid albumId, [FromBody] ReorderRequest request)
    {
        await _albumService.ReorderImagesAsync(albumId, request.Items);
        return NoContent();
    }
}
