using AuctionHouse.Core;
using AuctionHouse.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouse.Controllers;

public class AuctionsController : Controller
{
    private readonly IAuctionService _svc;
    public AuctionsController(IAuctionService svc) => _svc = svc;
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "demo-user";

    [HttpGet]
    public IActionResult Index()
    {
        var list = _svc.GetOngoing(DateTime.UtcNow)
                       .Select(AuctionListItemVm.FromAuction)
                       .ToList();
        return View(list);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var a = _svc.GetById(id);
        return View(AuctionVm.FromAuction(a));
    }

    [HttpGet]
    public IActionResult Create() => View(new CreateAuctionVm());

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(CreateAuctionVm vm)
    {
        if (!ModelState.IsValid) return View(vm);
        try
        {
            var id = _svc.Create(UserId, vm.Title, vm.Description, vm.StartPrice, vm.EndsAtUtc);
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(vm);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult EditDescription(int id, string description)
    {
        try
        {
            _svc.EditDescription(id, description, UserId, DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            TempData["EditError"] = ex.Message;
        }
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public IActionResult MyBids()
    {
        var list = _svc.GetUserOngoingBidAuctions(UserId, DateTime.UtcNow)
                       .Select(AuctionListItemVm.FromAuction)
                       .ToList();
        return View("Index", list);
    }

    [HttpGet]
    public IActionResult MyWins()
    {
        var list = _svc.GetFinishedWonByUser(UserId, DateTime.UtcNow)
                       .Select(AuctionListItemVm.FromAuction)
                       .ToList();
        return View("Index", list);
    }
}
