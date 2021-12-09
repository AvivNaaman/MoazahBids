using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoazahBids.Domain.Entities;
using MoazahBids.Web.Data;
using MoazahBids.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoazahBids.Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {

        private readonly IBidsService _bidsService;

        public IndexModel(IBidsService bidsService)
        {
            _bidsService = bidsService;
        }

        public async Task OnGet()
        {
            PageCount = await _bidsService.GetBidPageCount(PageSize);
            // selects valid offers, orders by total price
            Bids = await _bidsService.GetPagedBidsWithInfo(PageNum, PageSize);
        }

        public List<Bid> Bids { get; set; }

        const int PageSize = 50;

        [FromQuery]
        int PageNum { get; set; } = 1;
        public int PageCount { get; set; }
    }
}
