using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Events.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Registration> Registrations { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var eventBuilder = modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Category);
            entity.HasMany(e => e.Registrations).WithOne(r => r.Event);
            entity.HasOne(e => e.EventImage);

            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Describtion).HasMaxLength(255);
        });

        var categoriesBuilder = modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Url).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasMany(u => u.Registrations).WithOne(r => r.User);

            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.User).WithMany(u => u.Registrations);
            entity.HasOne(r => r.Event).WithMany(e => e.Registrations);
            entity.Property(r => r.RegistrationDate).IsRequired();
        });
    }
}
