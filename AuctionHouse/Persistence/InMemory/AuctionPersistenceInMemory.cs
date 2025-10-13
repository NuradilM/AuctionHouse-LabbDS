using System.Collections.Concurrent;
using AuctionHouse.Core.Interfaces;

namespace AuctionHouse.Persistence.InMemory;

public class AuctionPersistenceInMemory : IAuctionRepository
{
    // enkel trådsäkerhet med ConcurrentDictionary
    private static readonly ConcurrentDictionary<int, AuctionHouse.Core.Auction> _store = new();
    private static readonly List<AuctionHouse.Core.Bid> _bids = new();
    private static int _id = 1;
    private static int _bidId = 1;

    public void Add(AuctionHouse.Core.Auction auction)
    {
        var id = Interlocked.Increment(ref _id);
        auction.Id = id;
        _store[id] = auction;
    }

    public AuctionHouse.Core.Auction? GetById(int id, bool includeBids = false)
    {
        _store.TryGetValue(id, out var a);
        if (a == null) return null;

        if (includeBids)
        {
            var bids = _bids.Where(b => b.AuctionId == id)
                .OrderByDescending(b => b.Amount).ThenBy(b => b.PlacedAtUtc)
                .ToList();
            // “hydrate” domänen
            foreach (var b in bids)
                a.AddBid(b);
        }
        return a;
    }

    public List<AuctionHouse.Core.Auction> GetAll() => _store.Values.ToList();

    public void UpdateDescription(int auctionId, string newDesc)
    {
        if (_store.TryGetValue(auctionId, out var a))
            a.Description = newDesc;
        else
            throw new KeyNotFoundException("Auction not found");
    }

    public void AddBid(AuctionHouse.Core.Bid bid)
    {
        bid.Id = Interlocked.Increment(ref _bidId);
        _bids.Add(bid);

        // uppdatera domänens bid-list
        if (_store.TryGetValue(bid.AuctionId, out var a))
            a.AddBid(bid);
    }
}