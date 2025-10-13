using System.Collections.Generic;

namespace AuctionHouse.Core.Interfaces
{
    public interface IAuctionRepository
    {
        // Auction
        void Add(AuctionHouse.Core.Auction auction);
        AuctionHouse.Core.Auction? GetById(int id, bool includeBids = false);
        List<AuctionHouse.Core.Auction> GetAll();
        void UpdateDescription(int auctionId, string newDesc);

        // Bids
        void AddBid(AuctionHouse.Core.Bid bid);
    }
}