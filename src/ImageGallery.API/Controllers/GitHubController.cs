using Microsoft.AspNetCore.Mvc;
using ImageGallery.Application.Services;
using ImageGallery.Contracts;

namespace ImageGallery.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GitHubController : ControllerBase
{
    private readonly IGitHubImportService _gitHubService;

    public GitHubController(IGitHubImportService gitHubService) => _gitHubService = gitHubService;

    [HttpPost("preview")]
    public async Task<ActionResult<List<GitHubFileDto>>> Preview([FromBody] ImportRequest request)
    {
        var files = await _gitHubService.PreviewRepoAsync(request.RepoUrl, request.Path, request.Branch);
        return Ok(files);
    }
}
