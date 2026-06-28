using Microsoft.EntityFrameworkCore;
using ImageGallery.Domain.Entities;

namespace ImageGallery.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Image> Images => Set<Image>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<AlbumImage> AlbumImages => Set<AlbumImage>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlbumImage>()
            .HasKey(ai => new { ai.AlbumId, ai.ImageId });

        modelBuilder.Entity<AlbumImage>()
            .HasOne(ai => ai.Album)
            .WithMany(a => a.AlbumImages)
            .HasForeignKey(ai => ai.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AlbumImage>()
            .HasOne(ai => ai.Image)
            .WithMany(i => i.AlbumImages)
            .HasForeignKey(ai => ai.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.GitHubUserId)
            .IsUnique();
    }
}
