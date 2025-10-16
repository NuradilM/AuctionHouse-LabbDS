using System.ComponentModel.DataAnnotations;
using AuctionHouse.Core;

namespace AuctionHouse.Models.ViewModels;

public class AuctionVm
{
    [ScaffoldColumn(false)]
    public int Id { get; set; }

    public string Title { get; set; } = "";

    [Display(Name = "Description")]
    public string Description { get; set; } = "";

    public string SellerId { get; set; } = "";

    [Display(Name = "Ends (UTC)")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
    public DateTime EndsAt { get; set; }

    [Display(Name = "Start price")]
    [DisplayFormat(DataFormatString = "{0:0.00}")]
    public decimal StartPrice { get; set; }

    public List<BidVm> Bids { get; set; } = new();
    public decimal HighestBid { get; set; }

    public static AuctionVm FromAuction(Auction a)
    {
        return new AuctionVm
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            SellerId = a.SellerId,
            EndsAt = a.EndsAt,
            StartPrice = a.StartPrice,
            HighestBid = Math.Max(a.StartPrice, a.HighestBid()),
            Bids = a.Bids.Select(BidVm.FromBid).ToList()
        };
    }
}