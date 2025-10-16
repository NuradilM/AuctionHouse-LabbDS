namespace AuctionHouse.Core;

public interface IAuctionService
{
    // Queries
    List<Auction> GetOngoing(DateTime utcNow);
    
    List<Auction> GetEnded(DateTime utcNow);
    
    Auction GetById(int id);
    List<Auction> GetUserOngoingBidAuctions(string userId, DateTime utcNow);
    List<Auction> GetFinishedWonByUser(string userId, DateTime utcNow);

    // Commands
    int Create(string sellerId, string title, string description, decimal startPrice, DateTime endsAtUtc);
    void EditDescription(int auctionId, string newDescription, string userId, DateTime utcNow);
    void PlaceBid(int auctionId, decimal amount, string userId, DateTime utcNow);
}