using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ImageGallery.Application.Services;
using ImageGallery.Contracts;

namespace ImageGallery.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService) => _imageService = imageService;

    [HttpGet]
    public async Task<ActionResult<List<ImageDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] Guid? albumId)
    {
        return await _imageService.GetAllAsync(search, sort, albumId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ImageDto>> GetById(Guid id)
    {
        var image = await _imageService.GetByIdAsync(id);
        if (image is null) return NotFound();
        return image;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _imageService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("bulk-import")]
    public async Task<ActionResult<List<ImageDto>>> BulkImport([FromBody] BulkImportRequest request)
    {
        var images = await _imageService.BulkImportAsync(request.ImageUrls);
        return Ok(images);
    }
}
