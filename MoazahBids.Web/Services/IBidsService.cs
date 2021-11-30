using MoazahBids.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoazahBids.Web.Services
{
    public interface IBidsService
    {
        Task<bool> CancelOffer(int offerId);
        Task<Bid> CreateBidAsync(string name, string description, DateTime lastCallDate);
        Task EditBidItemAsync(string name, int bidId, int newQuantity, string newNotes);
        Task<int> GetBidPageCount(int pageSize);
        Task<List<Bid>> GetPagedBidsWithInfo(int pageNum, int pageSize);
        //Task<bool> SubmitBidOffer(int bidId, int supplierId, string supplierNotes, List<BidOfferItem> itemInfo);
    }
}