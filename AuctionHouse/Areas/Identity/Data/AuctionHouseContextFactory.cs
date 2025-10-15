using AuctionHouse.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AuctionHouseContextFactory : IDesignTimeDbContextFactory<AuctionHouseContext>
{
    public AuctionHouseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AuctionHouseContext>();
        var connectionString = configuration.GetConnectionString("IdentityConnection");
        
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AuctionHouseContext(optionsBuilder.Options);
    }
}