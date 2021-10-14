using Microsoft.AspNetCore.Mvc;
using MoazahBids.Domain.Entities;

namespace MoazahBids.Web.Controllers
{
    public class OffersController : Controller
    {
        [HttpGet]
        public IActionResult NewOffer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewOffer(BidOffer newOffer)
        {
            return View();
        }
    }
}
