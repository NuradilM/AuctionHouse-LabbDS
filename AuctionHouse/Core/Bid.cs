namespace AuctionHouse.Core;

public class Bid
{
    public int Id { get; set; }
    public int AuctionId { get; set; }
    public string BidderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PlacedAtUtc { get; set; }

    public Bid() { }
    public Bid(int auctionId, string bidderId, decimal amount, DateTime placedAtUtc)
    {
        AuctionId = auctionId;
        BidderId = bidderId;
        Amount = amount;
        PlacedAtUtc = placedAtUtc;
    }
}