namespace AuctionHouse.Core.Entities;

public class Auction
{
    public int Id { get; set; }
    public string SellerId { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal StartPrice { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public List<Bid> Bids { get; set; } = new List<Bid>();
}   