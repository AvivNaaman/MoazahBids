using System;
using System.Collections.Generic;
using System.Text;

namespace MoazahBids.Domain.Entities
{
    public class BidOfferItem
    {
        public string ItemName { get; set; }
        public int OfferId { get; set; }
        public int ProvidableQuantity { get; set; }
        public decimal? TotalTaxedPrice { get; set; }
        public BidOffer Offer { get; set; }
    }
}
