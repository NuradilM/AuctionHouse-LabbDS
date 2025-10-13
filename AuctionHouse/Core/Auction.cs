using AuctionHouse.Core;

namespace AuctionHouse.Core;

public class Auction
{
    public int Id { get; set; }
    public string SellerId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal StartPrice { get; set; }
    public DateTime EndsAtUtc { get; set; }

    private readonly List<Bid> _bids = new();
    public IEnumerable<Bid> Bids => _bids;

    public Auction() { } // behövs för persistence/EF
    public Auction(string sellerId, string title, string description, decimal startPrice, DateTime endsAtUtc)
    {
        SellerId = sellerId;
        Title = title;
        Description = description;
        StartPrice = startPrice;
        EndsAtUtc = endsAtUtc;
    }

    public void AddBid(Bid bid) => _bids.Add(bid);

    public bool IsOngoing(DateTime utcNow) => EndsAtUtc > utcNow;

    public decimal HighestBid() => _bids.Count == 0 ? 0m : _bids.Max(b => b.Amount);

    public bool IsWinner(string userId)
    {
        if (_bids.Count == 0) return false;
        var top = _bids.OrderByDescending(b => b.Amount).ThenBy(b => b.PlacedAtUtc).First();
        return top.BidderId == userId;
    }
}   