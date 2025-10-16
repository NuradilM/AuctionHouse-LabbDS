using System.ComponentModel.DataAnnotations;
using AuctionHouse.Core;

namespace AuctionHouse.Models.ViewModels;

public class AuctionListItemVm
{
    [ScaffoldColumn(false)]
    public int Id { get; set; }

    public string Title { get; set; } = "";

    [Display(Name = "Ends at")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime EndsAt { get; set; }

    [Display(Name = "Highest bid")]
    [DisplayFormat(DataFormatString = "{0:0.00}")]
    public decimal HighestBid { get; set; }

    public bool GetGoing { get; }
    public bool GetEnded { get; }
    
    public static AuctionListItemVm FromAuction(Auction a)
        => new()
        {
            Id = a.Id,
            Title = a.Title,
            EndsAt = a.EndsAt,
            HighestBid = Math.Max(a.StartPrice, a.HighestBid())
        };
}