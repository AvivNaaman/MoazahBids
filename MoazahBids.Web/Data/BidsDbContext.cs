using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoazahBids.Domain.Entities;

namespace MoazahBids.Web.Data
{
    public class BidsDbContext : IdentityDbContext<ApplicationUser>
    {
        public BidsDbContext(DbContextOptions<BidsDbContext> o) : base(o) { }


        public DbSet<Bid> Bids { get; set; }
        public DbSet<BidItem> BidsItems { get; set; }
        public DbSet<BidOffer> Offers { get; set; }
        public DbSet<BidOfferItem> OffersItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bid>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Name).IsRequired();
            });

            modelBuilder.Entity<BidItem>(e =>
            {
                e.HasKey(bi => new { bi.BidId, bi.Name });

                e.Property(bi => bi.RequiredQuantity).IsRequired();

                e.HasOne(bi => bi.Bid).WithMany(b => b.Items)
                    .HasForeignKey(bi => bi.BidId).IsRequired();
            });

            modelBuilder.Entity<BidOffer>(e =>
            {
                e.HasKey(bo => bo.Id);

                e.HasOne(bo => bo.Bid).WithMany(b => b.Offers)
                    .HasForeignKey(bo => bo.BidId).IsRequired();
            });

            modelBuilder.Entity<BidOfferItem>(e =>
            {
                e.HasKey(oi => new { oi.OfferId, oi.ItemName });

                e.HasOne(oi => oi.Offer).WithMany(o => o.ItemOffers)
                    .HasForeignKey(oi => oi.OfferId).IsRequired();
            });
        }

    }
}
