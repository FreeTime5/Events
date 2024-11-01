using Events.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Events.DataAccess;

public class ApplicationDbContext : IdentityDbContext<Member>
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
            entity.ToTable("Events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>()
                .ValueGeneratedOnAdd();
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
            entity.Property(e => e.ImageName)
                .IsRequired();
            entity.Property(e => e.Describtion)
                .HasMaxLength(255);
        });

        var categoriesBuilder = modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(c => c.Id);

            entity.Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>()
                .ValueGeneratedOnAdd();
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasMany(u => u.Registrations)
                .WithOne(r => r.Member);

            entity.Property(u => u.FirstName)
                .HasMaxLength(100);
            entity.Property(u => u.LastName)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.ToTable("Registrations");
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.Member)
                .WithMany(u => u.Registrations)
                .HasForeignKey(r => r.MemberId)
                .HasPrincipalKey(u => u.Id);
            entity.HasOne(r => r.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.EventId)
                .HasPrincipalKey(e => e.Id);

            entity.Navigation(r => r.Member)
                .AutoInclude();
            entity.Navigation(r => r.Event)
                .AutoInclude();
            entity.Property(e => e.Id)
                .HasValueGenerator<GuidValueGenerator>()
                .ValueGeneratedOnAdd();
            entity.Property(r => r.RegistrationDate)
                .IsRequired();
        });
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}

internal class GuidValueGenerator : ValueGenerator<string>
{
    public override bool GeneratesTemporaryValues { get; }

    public override string Next(EntityEntry entry)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        return Guid.NewGuid().ToString();
    }
}