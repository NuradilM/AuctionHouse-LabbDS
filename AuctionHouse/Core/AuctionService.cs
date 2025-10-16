using System.Data;
using AuctionHouse.Core.Interfaces;
using AuctionHouse.Persistence;

namespace AuctionHouse.Core;


public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _p;
    
    public List<Auction> AllAuction = new List<Auction>();

    public AuctionService(IAuctionRepository persistence) => _p = persistence;

    // Queries
    public List<Auction> GetOngoing(DateTime utcNow)
        => _p.GetAll().Where(a => a.IsOngoing(utcNow))
            .OrderBy(a => a.EndsAt).ToList();
    
    public List<Auction> GetEnded(DateTime utcNow)
        => _p.GetAll().Where(a => !a.IsOngoing(utcNow))
            .OrderByDescending(a => a.EndsAt).ToList();

    public Auction GetById(int id)
        => _p.GetById(id, includeBids: true) ?? throw new DataException("Auction not found");

    public List<Auction> GetUserOngoingBidAuctions(string userId, DateTime utcNow)
        => _p.GetAll()
             .Where(a => a.IsOngoing(utcNow) && a.Bids.Any(b => b.BidderId == userId))
             .OrderBy(a => a.EndsAt)
             .ToList();

    public List<Auction> GetFinishedWonByUser(string userId, DateTime utcNow)
        => _p.GetAll()
             .Where(a => !a.IsOngoing(utcNow) && a.IsWinner(userId))
             .OrderByDescending(a => a.EndsAt)
             .ToList();
    
    
    // Commands
    public int Create(string sellerId, string title, string description, decimal startPrice, DateTime endsAtUtc)
    {
        // validering likt lärarens stil (service-lagret)
        if (string.IsNullOrWhiteSpace(title) || title.Trim().Length is < 2 or > 100)
            throw new ArgumentException("Title must be 2–100 chars.");
        if (string.IsNullOrWhiteSpace(description) || description.Trim().Length is < 5 or > 2000)
            throw new ArgumentException("Description must be 5–2000 chars.");
        if (startPrice < 0) throw new ArgumentException("Start price must be >= 0.");
        if (endsAtUtc <= DateTime.Now) throw new ArgumentException("EndsAtUtc must be future UTC.");

        var a = new Auction(sellerId, title.Trim(), description.Trim(), startPrice, endsAtUtc);
        _p.Add(a);
        return a.Id; // persistence sätter Id
    }

    public void EditDescription(int auctionId, string newDescription, string userId, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(newDescription) || newDescription.Trim().Length is < 5 or > 2000)
            throw new ArgumentException("Description must be 5–2000 chars.");

        var a = _p.GetById(auctionId) ?? throw new DataException("Auction not found");
        if (!a.IsOngoing(utcNow)) throw new InvalidOperationException("Auction finished");
        if (a.SellerId != userId) throw new UnauthorizedAccessException("Only seller can edit");

        _p.UpdateDescription(auctionId, newDescription.Trim());
    }

    public void PlaceBid(int auctionId, decimal amount, string userId, DateTime utcNow)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be > 0.");

        var a = _p.GetById(auctionId, includeBids: true) ?? throw new DataException("Auction not found");
        if (!a.IsOngoing(utcNow)) throw new InvalidOperationException("Auction finished");
        if (a.SellerId == userId) throw new InvalidOperationException("Cannot bid on own auction");

        var minReq = Math.Max(a.StartPrice, a.HighestBid());
        if (amount <= minReq) throw new InvalidOperationException($"Bid must be > {minReq:0.00}");

        var bid = new Bid(auctionId, userId, amount, utcNow);
        _p.AddBid(bid);
    }
}
