namespace AuctionHouse.Core.DTOs;

public class BidDto
{
    public decimal Amount { get; set; }
    public string BidderId { get; set; } = "";
    public DateTime PlacedAtUtc { get; set; }
}