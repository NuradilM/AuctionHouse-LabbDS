using AuctionHouse.Core;
using AuctionHouse.Persistence;
using AuctionHouse.Core.Interfaces;   
using AuctionHouse.Persistence.InMemory;
using AuctionHouse.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.Core.Interfaces;
using AuctionHouse.Core;
    


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var auctionConn = builder.Configuration.GetConnectionString("AuctionConnection");
builder.Services.AddDbContext<AuctionDbContext>(opt =>
    opt.UseMySql(auctionConn, ServerVersion.AutoDetect(auctionConn)));

builder.Services.AddScoped<IAuctionRepository, AuctionRepositoryEf>();
builder.Services.AddScoped<IAuctionService, AuctionService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();