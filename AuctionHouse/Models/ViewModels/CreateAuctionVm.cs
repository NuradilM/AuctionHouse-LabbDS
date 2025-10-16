using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Models.ViewModels;

public class CreateAuctionVm
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = "";

    [Required, StringLength(2000, MinimumLength = 5)]
    public string Description { get; set; } = "";

    [Range(0, double.MaxValue)]
    public decimal StartPrice { get; set; }

    [Display(Name = "Ends")]
    [DataType(DataType.DateTime)]
    public DateTime EndsAt { get; set; } = DateTime.Now;
}