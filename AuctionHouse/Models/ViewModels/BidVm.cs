using System.ComponentModel.DataAnnotations;
using AuctionHouse.Core;

namespace AuctionHouse.Models.ViewModels;

public class BidVm
{
    [DisplayFormat(DataFormatString = "{0:0.00}")]
    public decimal Amount { get; set; }

    public string BidderId { get; set; } = "";

    [Display(Name = "Placed (UTC)")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime PlacedAtUtc { get; set; }

    public static BidVm FromBid(Bid b) => new()
    {
        Amount = b.Amount,
        BidderId = b.BidderId,
        PlacedAtUtc = b.PlacedAtUtc
    };
}