using Octokit;
using ImageGallery.Contracts;

namespace ImageGallery.Application.Services;

public interface IGitHubImportService
{
    Task<List<GitHubFileDto>> PreviewRepoAsync(string repoUrl, string? path, string? branch);
}

public class GitHubImportService : IGitHubImportService
{
    private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".gif", ".webp", ".svg", ".bmp", ".ico"
    };

    private readonly GitHubClient _client;

    public GitHubImportService(GitHubClient client)
    {
        _client = client;
    }

    public async Task<List<GitHubFileDto>> PreviewRepoAsync(string repoUrl, string? path, string? branch)
    {
        var (owner, repo) = ParseRepoUrl(repoUrl);
        if (string.IsNullOrWhiteSpace(owner) || string.IsNullOrWhiteSpace(repo))
            return new List<GitHubFileDto>();

        path ??= string.Empty;
        branch ??= "main";

        try
        {
            var contents = await _client.Repository.Content.GetAllContents(owner, repo, path);
            var imageFiles = contents
                .Where(c => c.Type == ContentType.File && ImageExtensions.Contains(Path.GetExtension(c.Name)))
                .Select(c => new GitHubFileDto
                {
                    Name = c.Name,
                    DownloadUrl = c.DownloadUrl,
                    SizeBytes = c.Size
                }).ToList();

            return imageFiles;
        }
        catch
        {
            return new List<GitHubFileDto>();
        }
    }

    private static (string? owner, string? repo) ParseRepoUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Trim('/').Split('/');
            if (segments.Length >= 2)
                return (segments[0], segments[1]);
        }
        catch { }

        return (null, null);
    }
}
