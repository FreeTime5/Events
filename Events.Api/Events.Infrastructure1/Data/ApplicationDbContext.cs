using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;


namespace Events.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
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
            entity.Property(e => e.Id).HasValueGenerator<KekValueGenerator>();
            entity.HasOne(e => e.Category)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(e => e.CategoryId)
                .HasPrincipalKey(c => c.Id);
            entity.HasMany(e => e.Registrations).WithOne(r => r.Event);
            entity.HasOne(e => e.Creator)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(e => e.CreatorId)
                .HasPrincipalKey(u => u.Id);

            entity.Navigation(ev => ev.Category)
                .AutoInclude();
            entity.Navigation(ev => ev.Creator)
                .AutoInclude();
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ImageUrl)
                .IsRequired();
            entity.Property(e => e.Describtion)
                .HasMaxLength(255);
        });

        var categoriesBuilder = modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasMany(u => u.Registrations)
                .WithOne(r => r.User);

            entity.Property(u => u.FirstName)
                .HasMaxLength(100);
            entity.Property(u => u.LastName)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.User)
                .WithMany(u => u.Registrations);
            entity.HasOne(r => r.Event)
                .WithMany(e => e.Registrations);
            entity.Property(r => r.RegistrationDate)
                .IsRequired();
        });
    }
}

public class KekValueGenerator : ValueGenerator
{
    public override bool GeneratesTemporaryValues => false;

    protected override object? NextValue(EntityEntry entry)
    {
        return Guid.NewGuid().ToString();
    }
}
