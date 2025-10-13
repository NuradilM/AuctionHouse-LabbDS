using AuctionHouse.Core;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouse.Controllers;

public class BidsController : Controller
{
    private readonly IAuctionService _svc;
    public BidsController(IAuctionService svc) => _svc = svc;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "demo-user";

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Place(int auctionId, decimal amount)
    {
        try
        {
            _svc.PlaceBid(auctionId, amount, UserId, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            TempData["BidError"] = ex.Message;
        }
        return RedirectToAction("Details", "Auctions", new { id = auctionId });
    }
}