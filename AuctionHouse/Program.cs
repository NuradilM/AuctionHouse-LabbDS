using AuctionHouse.Areas.Identity.Data;
using AuctionHouse.Core;
using AuctionHouse.Persistence;
using AuctionHouse.Core.Interfaces;
using AuctionHouse.Persistence.InMemory;
using AuctionHouse.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var auctionConn = builder.Configuration.GetConnectionString("AuctionConnection");
builder.Services.AddDbContext<AuctionDbContext>(opt =>
    opt.UseMySql(auctionConn, ServerVersion.AutoDetect(auctionConn)));

var identityConn = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<AuctionHouseContext>(options =>
    options.UseMySql(identityConn, ServerVersion.AutoDetect(identityConn)));

builder.Services.AddDefaultIdentity<AuctionHouseUser>(opt =>
    {
        opt.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AuctionHouseContext>()
    .AddDefaultUI();

builder.Services.AddRazorPages();

builder.Services.AddScoped<IAuctionRepository, AuctionRepositoryEf>();
builder.Services.AddScoped<IAuctionService, AuctionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();