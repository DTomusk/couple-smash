using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Pairing> Pairings => Set<Pairing>();
    public DbSet<Rating> Ratings => Set<Rating>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Pairing>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.FirstMember)
                .WithMany()
                .HasForeignKey(e => e.FirstMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.SecondMember)
                .WithMany()
                .HasForeignKey(e => e.SecondMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.IsExempted)
                .IsRequired();
            entity.Property(e => e.CompatibilityRating)
                .IsRequired();
            entity.Property(e => e.NumberOfRatings)
                .IsRequired();
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PairingId)
                .IsRequired();
            entity.Property(e => e.Value)
                .IsRequired();
        });
    }
}
