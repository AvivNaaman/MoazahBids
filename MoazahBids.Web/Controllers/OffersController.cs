using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoazahBids.Domain.Entities;
using MoazahBids.Web.Data;
using MoazahBids.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoazahBids.Web.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private readonly BidsDbContext db;
        private readonly IWebHostEnvironment env;

        public OffersController(BidsDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        //[Authorize(Roles = "Supplier")]
        [HttpGet]
        public async Task<IActionResult> NewOffer(int id)
        {
            var b = await db.Bids.Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (b is null) return NotFound();
            var emptyModel = new NewOfferModel
            {
                Bid = b
            };
            return View(emptyModel);
        }

        //[Authorize(Roles = "Supplier")]
        [HttpPost]
        public async Task<IActionResult> NewOffer(int id, NewOfferModel offerModel)
        {
            // validate and submit
            if (!ModelState.IsValid)
            {
                return View(offerModel);

            }

            var itemsToFill = await db.BidsItems
                .Where(bi => bi.BidId == id).ToListAsync();


            var newOffer = new BidOffer()
            {
                BidId = id,
                Status = BidOfferStatus.Relevant,
                IsComplete = CheckComplete(offerModel.ItemNames, offerModel.Prices, itemsToFill),
                SupplierNotes = offerModel.SupplierNotes,
                TotalTaxedPrice = CalcTotalPrice(offerModel.Prices),
                SupplierName = offerModel.SupplierName,
                FileName = id + Path.GetExtension(offerModel.BidFile.FileName)
            };

            using (var t = await db.Database.BeginTransactionAsync())
            {
                db.Add(newOffer);
                await db.SaveChangesAsync();
                for (var i = 0; i < itemsToFill.Count; i++)
                {
                    db.OffersItems.Add(new BidOfferItem()
                    {
                        OfferId = newOffer.Id,
                        ItemName = offerModel.ItemNames[i],
                        ProvidableQuantity = itemsToFill.First(itf => itf.Name == offerModel.ItemNames[i]).RequiredQuantity,
                        TotalTaxedPrice = offerModel.Prices[i]
                    });
                }
                await db.SaveChangesAsync();
                // save bid file
                var filePath = Path.Combine(env.WebRootPath, "BidFiles", newOffer.FileName); // wwwroot\BidFiles\id.pdf
                using (var stream = System.IO.File.Create(filePath))
                {
                    await offerModel.BidFile.CopyToAsync(stream);
                }
                t.Commit();
            }

            return RedirectToAction("Details", "Bids", new { id });
        }

        private decimal CalcTotalPrice(List<decimal?> list)
        {
            return list.Where(i => i.HasValue).Cast<decimal>().Sum();
        }

        public async Task<IActionResult> Details(int id)
        {
            var o = await db.Offers.Include(o => o.ItemOffers).FirstOrDefaultAsync(o => o.Id == id);
            return o is null ? NotFound() : View(o);
        }

        private bool CheckComplete(List<string> names, List<decimal?> prices, List<BidItem> toFill) =>
            toFill.All(tf =>
                Enumerable.Range(0, names.Count)
                .Any(i => names[i] == tf.Name && prices[i].HasValue)
            );
    }
}
