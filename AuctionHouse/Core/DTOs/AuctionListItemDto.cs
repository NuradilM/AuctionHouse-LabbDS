namespace AuctionHouse.Core.DTOs;

public class AuctionListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public DateTime EndsAtUtc { get; set; }
    public decimal HighestBid { get; set; }
}