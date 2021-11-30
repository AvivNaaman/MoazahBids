using MoazahBids.Domain.Entities;
using MoazahBids.Web.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace MoazahBids.Web.Services
{
    public class BidsService : IBidsService
    {
        private readonly BidsDbContext db;

        public BidsService(BidsDbContext db)
        {
            this.db = db;
        }

        public async Task<Bid> CreateBidAsync(string name, string? description, DateTime lastCallDate)
        {
            var toReturn = new Bid
            {
                Name = name,
                Description = description,
                CreatedDate = DateTime.Now,
                LastCallDate = lastCallDate,
                IsHidden = true
            };
            db.Add(toReturn);
            await db.SaveChangesAsync();
            return toReturn;
        }

        public async Task EditBidItemAsync(string name, int bidId, int newQuantity, string newNotes)
        {
            if (newQuantity is 0)
            {
                // remove itme if exists
                db.RemoveRange(db.BidsItems.Where(bi => bi.BidId == bidId));
            }
            else
            {
                var qi = await db.BidsItems.FirstOrDefaultAsync(bi => bi.BidId == bidId && bi.Name == name);
                if (qi is not null)
                {
                    qi.RequiredQuantity = newQuantity;
                    qi.Notes = newNotes;
                    db.Update(qi);
                }
                else
                {
                    qi = new() { BidId = bidId, Name = name, Notes = newNotes, RequiredQuantity = newQuantity };
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> SubmitBidOffer(int bidId, int supplierId, string supplierNotes, List<BidOfferItem> itemInfo)
        {
            var o = await db.Offers.FirstOrDefaultAsync(o => o.BidId == bidId && o.SupplierId == supplierId);
            if (o is not null)
            {
                return false;
            }
            using (var ts = await db.Database.BeginTransactionAsync())
            {
                itemInfo.ForEach(ii => ii.OfferId = o.Id);
                o = new()
                {
                    BidId = bidId,
                    SupplierId = supplierId,
                    SupplierNotes = supplierNotes,
                    Status = BidOfferStatus.Relevant,
                };
                await UpdateOfferDataByItems(o, itemInfo);
                await db.AddAsync(o);
                await db.SaveChangesAsync();
                await db.AddRangeAsync(itemInfo);
                await db.SaveChangesAsync();
                await ts.CommitAsync();
            }
            return true;
        }

        /// <summary>
        /// Updates the offer is complete/total taxed price
        /// No db store!
        /// </summary>
        private async Task UpdateOfferDataByItems(BidOffer offer, List<BidOfferItem> items)
        {
            var requiredItems = await db.BidsItems.Where(bi => bi.BidId == offer.BidId).ToListAsync();

            offer.IsComplete = requiredItems.Select(ri => ri.Name).Except(items.Select(ii => ii.ItemName)).Count() == 0 &&
                        requiredItems.All(ri => items.Any(ii => ii.ProvidableQuantity == ri.RequiredQuantity));
            //offer.TotalTaxedPrice = items.Sum(ii => ii.TotalTaxedPrice);
        }

        public async Task<bool> CancelOffer(int offerId)
        {
            var o = await db.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
            if (o is null) return false;
            o.Status = BidOfferStatus.Canceled;

            return true;
        }

        /// <summary>
        /// Returns the pageNum page of pageSize size of bids, including the top 3 bids, ordered by completion and then pricing.
        /// </summary>
        public async Task<List<Bid>> GetPagedBidsWithInfo(int pageNum, int pageSize)
        {
            var toReturn = await db.Bids.Include(b => b.Offers).ToListAsync();
            toReturn.ForEach(b => b.Offers.OrderBy(o => o.IsComplete).ThenBy(o => o.TotalTaxedPrice).Take(3));
            return toReturn;
        }

        
        public async Task<int> GetBidPageCount(int pageSize)
            => (int)Math.Ceiling(await db.Bids.CountAsync() / (double)pageSize);
    }
}
