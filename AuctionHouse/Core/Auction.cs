using AuctionHouse.Core;

namespace AuctionHouse.Core;

public class Auction
{
    public int Id { get; set; }
    public string SellerId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal StartPrice { get; set; }
    public DateTime EndsAt { get; set; }

    private readonly List<Bid> _bids = new();
    // public List<Bid> Bids { get; set; } = new();

    public Auction() { } 
    public Auction(string sellerId, string title, string description, decimal startPrice, DateTime endsAt)
    {
        SellerId = sellerId;
        Title = title;
        Description = description;
        StartPrice = startPrice;
        EndsAt = endsAt;
    }
    
    public List<Bid> Bids
    {
        get => _bids
            .OrderByDescending(b => b.Amount)
            .ThenBy(b => b.PlacedAtUtc)
            .ToList();
        set
        {
            _bids.Clear();
            if (value != null) _bids.AddRange(value);
        }
    }
    
    public void AddBid(Bid bid) => _bids.Add(bid);

    public bool IsOngoing(DateTime tNow) => EndsAt > tNow;
    
    // Auction.cs
    public bool IsEnded(DateTime tNow) => EndsAt <= tNow;


    public decimal HighestBid() => _bids.Count == 0 ? 0m : _bids.Max(b => b.Amount);

    public bool IsWinner(string userId)
    {
        if (_bids.Count == 0) return false;
        var top = _bids.OrderByDescending(b => b.Amount).ThenBy(b => b.PlacedAtUtc).First();
        return top.BidderId == userId;
    }
}   