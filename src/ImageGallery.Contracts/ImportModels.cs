namespace ImageGallery.Contracts;

public class ImportRequest
{
    public string RepoUrl { get; set; } = string.Empty;
    public string? Path { get; set; }
    public string? Branch { get; set; }
}

public class GitHubFileDto
{
    public string Name { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
}

public class ReorderRequest
{
    public List<ReorderItem> Items { get; set; } = new();
}

public class ReorderItem
{
    public Guid ImageId { get; set; }
    public int SortOrder { get; set; }
}

public class CreateAlbumRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateAlbumRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
}
