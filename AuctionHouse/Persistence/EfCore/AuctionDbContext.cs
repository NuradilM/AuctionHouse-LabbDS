using AuctionHouse.Core;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Persistence.EfCore;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

    public DbSet<Auction> Auctions => Set<Auction>();
    public DbSet<Bid> Bids => Set<Bid>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.HasCharSet("utf8mb4").UseCollation("utf8mb4_unicode_ci");

        b.Entity<Auction>(e =>
        {
            e.Property(x => x.Title).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).IsRequired().HasMaxLength(2000);
            e.Property(x => x.StartPrice).HasPrecision(18, 2);
            e.HasIndex(x => x.EndsAt);
        });

        b.Entity<Bid>(e =>
        {
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.HasOne<Auction>()
                .WithMany(a => (ICollection<Bid>)a.Bids)
                .HasForeignKey(x => x.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => x.AuctionId);
        });
    }
}