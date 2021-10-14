using System;
using System.Collections.Generic;
using System.Text;

namespace MoazahBids.Domain.Entities
{
    /// <summary>
    /// Respresents a single item in the item list of a bid.
    /// </summary>
    public class BidItem
    {
        public int BidId { get; set; }
        public Bid Bid { get; set; }
        public string Name { get; set; }
        public int RequiredQuantity { get; set; }
        public string? Notes { get; set; }
    }
}
