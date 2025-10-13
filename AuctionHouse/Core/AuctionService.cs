using AuctionHouse.Core.DTOs;
using AuctionHouse.Core.Entities;
using AuctionHouse.Core.Interfaces;

namespace AuctionHouse.Core.Services;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctions;
    private readonly IBidRepository _bids;

    public AuctionService(IAuctionRepository auctions, IBidRepository bids)
    {
        _auctions = auctions;
        _bids = bids;
    }

    public int CreateAuction(CreateAuctionDto dto, string userId)
    {
        if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Trim().Length is < 2 or > 100)
            throw new ArgumentException("Title must be 2-100 chars.");
        if (string.IsNullOrWhiteSpace(dto.Description) || dto.Description.Trim().Length is < 5 or > 2000)
            throw new ArgumentException("Description must be 5-2000 chars.");
        if (dto.StartPrice < 0) throw new ArgumentException("Start price must be >= 0.");
        if (dto.EndsAtUtc <= DateTime.UtcNow) throw new ArgumentException("EndsAtUtc must be in the future (UTC).");

        var a = new Auction
        {
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            StartPrice = dto.StartPrice,
            EndsAtUtc = dto.EndsAtUtc,
            SellerId = userId
        };
        _auctions.Add(a);
        return a.Id;
    }

    public void EditDescription(int auctionId, string newDescription, string userId, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(newDescription) || newDescription.Trim().Length is < 5 or > 2000)
            throw new ArgumentException("Description must be 5-2000 chars.");

        var a = _auctions.GetById(auctionId);
        if (a is null) throw new InvalidOperationException("Auction not found.");
        if (a.SellerId != userId) throw new UnauthorizedAccessException("Only seller can edit.");
        if (a.EndsAtUtc <= utcNow) throw new InvalidOperationException("Auction finished.");

        _auctions.UpdateDescription(auctionId, newDescription.Trim());
    }

    public IReadOnlyList<AuctionListItemDto> GetOngoing(DateTime utcNow, int page = 1, int pageSize = 20)
        => _auctions.GetOngoing(utcNow, page, pageSize);

    public AuctionDetailsDto? GetDetails(int auctionId, DateTime utcNow)
    {
        var a = _auctions.GetById(auctionId, includeBids: true);
        if (a is null) return null;
        return new AuctionDetailsDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            SellerId = a.SellerId,
            EndsAtUtc = a.EndsAtUtc,
            StartPrice = a.StartPrice,
            Bids = a.Bids.OrderByDescending(b => b.Amount)
                         .Select(b => new BidDto { Amount = b.Amount, BidderId = b.BidderId, PlacedAtUtc = b.PlacedAtUtc })
                         .ToList()
        };
    }

    public IReadOnlyList<AuctionListItemDto> GetUserOngoingBidAuctions(string userId, DateTime utcNow)
        => _auctions.GetUserOngoingBidAuctions(userId, utcNow);

    public IReadOnlyList<AuctionListItemDto> GetFinishedWonByUser(string userId, DateTime utcNow)
        => _auctions.GetFinishedWonByUser(userId, utcNow);
}