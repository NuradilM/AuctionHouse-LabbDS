namespace AuctionHouse.Core.DTOs;

public class CreateAuctionDto
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal StartPrice { get; set; }
    public DateTime EndsAtUtc { get; set; } // UTC
}