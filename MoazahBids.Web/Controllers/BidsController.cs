﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoazahBids.Domain.Entities;
using MoazahBids.Web.Data;
using MoazahBids.Web.Models;

namespace MoazahBids.Web.Controllers
{
    public class BidsController : Controller
    {
        private readonly BidsDbContext _context;

        public BidsController(BidsDbContext context)
        {
            _context = context;
        }

        // GET: Bids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids.Include(b => b.Items).Include(b => b.Offers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // GET: Bids/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsHidden,CreatedDate,LastCallDate")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                bid.CreatedDate = DateTime.Now;
                _context.Add(bid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bid);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([Bind("BidId,Name,RequiredQuantity,Notes")] BidItemModel itemOld, int? id)
        {
            if (ModelState.IsValid)
            {
                BidItem item = new BidItem
                {
                    BidId = itemOld.BidId,
                    Name = itemOld.Name,
                    RequiredQuantity = itemOld.RequiredQuantity,
                    Notes = itemOld.Notes
                };
                if (BidExists(item.BidId) &&
                    !await _context.BidsItems.AnyAsync(b =>
                        b.Name == item.Name && b.BidId == item.BidId))
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                }
                else ModelState.AddModelError("", "הפריט כבר קיים");
            }
            return await Edit(id);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(string name, int bidId)
        {
            _context.RemoveRange(_context.BidsItems.Where(bi => bi.Name == name && bidId == bi.BidId));
            var r = await _context.SaveChangesAsync();
            return r is 1 ? Ok() : NotFound();
        }

        // GET: Bids/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids.Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (bid == null)
            {
                return NotFound();
            }
            return View(bid);
        }

        // POST: Bids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsHidden,LastCallDate")] Bid bid)
        {
            if (id != bid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BidExists(bid.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bid);
        }

        // GET: Bids/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // POST: Bids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BidExists(int id)
        {
            return _context.Bids.Any(e => e.Id == id);
        }
    }
}