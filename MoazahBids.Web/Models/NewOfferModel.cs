using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoazahBids.Domain.Entities;
using MoazahBids.Web.Helpers;
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
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)] // 5mb
        [AllowedExtensions(new string[] { ".jpg",".jpeg", ".pdf", ".png" })]
        [FromForm]
        public IFormFile BidFile { get; set; }
        [Required]
        [MinLength(5)]

        public string SupplierName { get; set; }
    }
}
