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

            entity.HasOne<Member>()
                .WithMany()
                .HasForeignKey(e => e.FirstMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Member>()
                .WithMany()
                .HasForeignKey(e => e.SecondMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.FirstMemberId)
                .IsRequired();
            entity.Property(e => e.SecondMemberId)
                .IsRequired();
            entity.Property(e => e.IsExempted)
                .IsRequired();
            entity.Property(e => e.CompatibilityRating)
                .IsRequired();
            entity.Property(e => e.NumberOfRatings)
                .IsRequired();
        });
    }
}
