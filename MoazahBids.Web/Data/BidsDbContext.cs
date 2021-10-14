using Microsoft.EntityFrameworkCore;
using MoazahBids.Domain.Entities;

namespace MoazahBids.Web.Data
{
    public partial class BidsDbContext : DbContext
    {
        public BidsDbContext(DbContextOptions<BidsDbContext> o) : base(o) { }


        public DbSet<Bid> Bids { get; set; }
        public DbSet<BidItem> BidsItems { get; set; }
        public DbSet<BidOffer> Offers { get; set; }
        public DbSet<BidOfferItem> OffersItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder builder);
    }
}
