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
        var parsed = ParseGitHubUrl(repoUrl);
        if (parsed.owner is null || parsed.repo is null)
            return new List<GitHubFileDto>();

        // Use provided path/branch, fall back to parsed from URL, then defaults
        path ??= parsed.path ?? string.Empty;
        branch ??= parsed.branch ?? "main";

        try
        {
            var contents = await _client.Repository.Content.GetAllContents(parsed.owner, parsed.repo, path);
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

    private static (string? owner, string? repo, string? branch, string? path) ParseGitHubUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Trim('/').Split('/');
            if (segments.Length < 2)
                return (null, null, null, null);

            var owner = segments[0];
            var repo = segments[1];

            // URL patterns:
            //   https://github.com/owner/repo
            //   https://github.com/owner/repo/tree/branch/path
            //   https://github.com/owner/repo/blob/branch/path
            //   https://github.com/owner/repo/tree/branch
            string? branch = null;
            string? path = null;

            if (segments.Length > 2)
            {
                var type = segments[2]; // "tree" or "blob"
                if (type is "tree" or "blob" && segments.Length > 3)
                {
                    branch = segments[3];
                    if (segments.Length > 4)
                        path = string.Join("/", segments.Skip(4));
                }
            }

            return (owner, repo, branch, path);
        }
        catch
        {
            return (null, null, null, null);
        }
    }
}
