namespace AuctionHouse.Core.DTOs;

public class AuctionDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string SellerId { get; set; } = "";
    public DateTime EndsAtUtc { get; set; }
    public decimal StartPrice { get; set; }
    public List<BidDto> Bids { get; set; } = new List<BidDto>();
}