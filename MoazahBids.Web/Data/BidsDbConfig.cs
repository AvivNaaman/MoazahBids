using Microsoft.EntityFrameworkCore;
using MoazahBids.Domain.Entities;

namespace MoazahBids.Web.Data
{
    public partial class BidsDbContext: DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder builder)
        {
            builder.Entity<Bid>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Name).IsRequired();
            });

            builder.Entity<BidItem>(e =>
            {
                e.HasKey(bi => new { bi.BidId, bi.Name });

                e.Property(bi => bi.RequiredQuantity).IsRequired();

                e.HasOne(bi => bi.Bid).WithMany(b => b.Items)
                    .HasForeignKey(bi => bi.BidId).IsRequired();
            });

            builder.Entity<BidOffer>(e =>
            {
                e.HasKey(bo => bo.Id);

                e.HasOne(bo => bo.Bid).WithMany(b => b.Offers)
                    .HasForeignKey(bo => bo.BidId).IsRequired();
            });

            builder.Entity<BidOfferItem>(e =>
            {
                e.HasKey(oi => new { oi.OfferId, oi.ItemName });

                e.HasOne(oi => oi.Offer).WithMany(o => o.ItemOffers)
                    .HasForeignKey(oi => oi.OfferId).IsRequired();
            });
        }
    }
}
