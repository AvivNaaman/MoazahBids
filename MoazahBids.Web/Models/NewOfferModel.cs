using MoazahBids.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoazahBids.Web.Models
{
    public class NewOfferModel
    {
        public Bid Bid { get; set; }
        public bool IsPriceWithTax { get; set; } = true;
        public List<decimal?> Prices { get; set; }
        public List<string> ItemNames { get; set; }
        public string SupplierNotes { get; set; }
    }
}
