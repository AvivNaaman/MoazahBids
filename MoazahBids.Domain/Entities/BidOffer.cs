using System;
using System.Collections.Generic;
using System.Text;

namespace MoazahBids.Domain.Entities
{
    /// <summary>
    /// Represents a single offer from a certain supplier for a bid.
    /// </summary>
    public class BidOffer
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string? SupplierNotes { get; set; }
        public int BidId { get; set; }
        public Bid Bid { get; set; }
        public BidOfferStatus Status { get; set; }
        public List<BidOfferItem> ItemOffers { get; set; }
        public bool IsComplete { get; set; }
        public decimal TotalTaxedPrice { get; set; }
        public string FileName { get; set; }
    }

    public enum BidOfferStatus
    {
        Relevant,
        Canceled,
        Declined
    }
}
