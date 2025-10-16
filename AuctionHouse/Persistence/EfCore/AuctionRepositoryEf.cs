using AuctionHouse.Core;
using AuctionHouse.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Persistence.EfCore;

public class AuctionRepositoryEf : IAuctionRepository
{
    private readonly AuctionDbContext _db;
    public AuctionRepositoryEf(AuctionDbContext db) => _db = db;

    // Commands
    public void Add(Auction auction)
    {
        _db.Auctions.Add(auction);
        _db.SaveChanges();
    }

    public void UpdateDescription(int auctionId, string newDescription)
    {
        var a = _db.Auctions.FirstOrDefault(x => x.Id == auctionId)
                ?? throw new InvalidOperationException("Auction not found");
        a.Description = newDescription;
        _db.SaveChanges();
    }

    public void AddBid(Bid bid)
    {
        _db.Bids.Add(bid);
        _db.SaveChanges();
    }

    // Queries
    public Auction? GetById(int id, bool includeBids = false)
    {
        IQueryable<Auction> q = _db.Auctions.AsNoTracking();
        if (includeBids)
            q = q.Include(a => a.Bids).AsSplitQuery();
        return q.FirstOrDefault(a => a.Id == id);
    }

    public List<Auction> GetAll()
        => _db.Auctions
            .AsNoTracking()
            .Include(a => a.Bids)
            .AsSplitQuery()
            .ToList();
    
}