using System;
using System.Collections.Generic;
using System.Text;

namespace MoazahBids.Domain.Entities
{
    /// <summary>
    /// Respresents a single bid
    /// </summary>
    public class Bid
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsHidden { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastCallDate { get; set; }

        public List<BidItem>? Items { get; set; }

        public List<BidOffer>? Offers { get; set; }
    }
}
