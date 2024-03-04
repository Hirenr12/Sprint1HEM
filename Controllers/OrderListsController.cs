using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sprint1HEM.Models;

namespace Sprint1HEM.Controllers
{
    public class OrderListsController : Controller
    {
        private readonly RacersContext _context;

        public OrderListsController(RacersContext context)
        {
            _context = context;
        }

        // GET: OrderLists
        public async Task<IActionResult> Index()
        {
            var racersContext = _context.OrderLists.Include(o => o.Customer).Include(o => o.Item);
            return View(await racersContext.ToListAsync());
        }

        // GET: OrderLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderList = await _context.OrderLists
                .Include(o => o.Customer)
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderList == null)
            {
                return NotFound();
            }

            return View(orderList);
        }

        // GET: OrderLists/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId");
            return View();
        }

        // POST: OrderLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,ItemId,Price")] OrderList orderList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orderList.CustomerId);
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", orderList.ItemId);
            return View(orderList);
        }

        // GET: OrderLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderList = await _context.OrderLists.FindAsync(id);
            if (orderList == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orderList.CustomerId);
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", orderList.ItemId);
            return View(orderList);
        }

        // POST: OrderLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,ItemId,Price")] OrderList orderList)
        {
            if (id != orderList.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderListExists(orderList.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orderList.CustomerId);
            ViewData["ItemId"] = new SelectList(_context.Items, "ItemId", "ItemId", orderList.ItemId);
            return View(orderList);
        }

        // GET: OrderLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderList = await _context.OrderLists
                .Include(o => o.Customer)
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderList == null)
            {
                return NotFound();
            }

            return View(orderList);
        }

        // POST: OrderLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderList = await _context.OrderLists.FindAsync(id);
            if (orderList != null)
            {
                _context.OrderLists.Remove(orderList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderListExists(int id)
        {
            return _context.OrderLists.Any(e => e.OrderId == id);
        }
    }
}
