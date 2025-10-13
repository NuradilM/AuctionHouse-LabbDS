using System.ComponentModel.DataAnnotations;
using AuctionHouse.Core;

namespace AuctionHouse.Models.ViewModels;

public class AuctionListItemVm
{
    [ScaffoldColumn(false)]
    public int Id { get; set; }

    public string Title { get; set; } = "";

    [Display(Name = "Ends (UTC)")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime EndsAtUtc { get; set; }

    [Display(Name = "Highest bid")]
    [DisplayFormat(DataFormatString = "{0:0.00}")]
    public decimal HighestBid { get; set; }

    public static AuctionListItemVm FromAuction(Auction a)
        => new()
        {
            Id = a.Id,
            Title = a.Title,
            EndsAtUtc = a.EndsAtUtc,
            HighestBid = Math.Max(a.StartPrice, a.HighestBid())
        };
}