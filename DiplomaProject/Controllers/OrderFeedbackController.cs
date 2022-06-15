using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;

namespace DiplomaProject.Controllers
{
    public class OrderFeedbackController : Controller
    {
        private readonly KraftWebAppContext _context;

        public OrderFeedbackController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: OrderFeedback
        public async Task<IActionResult> Index()
        {
            var kraftWebAppContext = _context.OrderFeedbacks.Include(o => o.Customer).Include(o => o.Order);
            return View(await kraftWebAppContext.ToListAsync());
        }

        // GET: OrderFeedback/Details/5
        public async Task<IActionResult> Details(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }

            var orderFeedback = await _context.OrderFeedbacks
                .Include(o => o.Customer)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderId == orderId);
            if (orderFeedback == null)
            {
                return NotFound();
            }

            return View(orderFeedback);
        }

        // GET: OrderFeedback/Create
        public IActionResult Create(int orderId, int customerId)
        {
            ViewData["CustomerId"] = customerId;//new SelectList(_context.Users, "Id", "Id");
            ViewData["OrderId"] = orderId;//new SelectList(_context.Orders, "Id", "Id");
            return View();
        }

        // POST: OrderFeedback/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,Estimation,IsInTime,CustomerId,OrderId")] OrderFeedback orderFeedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderFeedback);

                var order = await _context.Orders.FindAsync(orderFeedback.OrderId);
                order.ReadyStageId = 10;
                _context.Update(order);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Order", new {id = orderFeedback.OrderId});
            }
            // ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", orderFeedback.CustomerId);
            // ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderFeedback.OrderId);
            return View(orderFeedback);
        }

        // GET: OrderFeedback/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderFeedback = await _context.OrderFeedbacks.FindAsync(id);
            if (orderFeedback == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", orderFeedback.CustomerId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderFeedback.OrderId);
            return View(orderFeedback);
        }

        // POST: OrderFeedback/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Estimation,IsInTime,CustomerId,OrderId")] OrderFeedback orderFeedback)
        {
            if (id != orderFeedback.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderFeedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderFeedbackExists(orderFeedback.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), "Order", new {id = orderFeedback.OrderId});
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", orderFeedback.CustomerId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderFeedback.OrderId);
            return View(orderFeedback);
        }

        // GET: OrderFeedback/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderFeedback = await _context.OrderFeedbacks
                .Include(o => o.Customer)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderFeedback == null)
            {
                return NotFound();
            }

            return View(orderFeedback);
        }

        // POST: OrderFeedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderFeedback = await _context.OrderFeedbacks.FindAsync(id);
            _context.OrderFeedbacks.Remove(orderFeedback);
            
            var order = await _context.Orders.FindAsync(orderFeedback.OrderId);
            order.ReadyStageId = 9;
            _context.Update(order);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "Order", new {id = orderFeedback.OrderId});
        }

        private bool OrderFeedbackExists(int id)
        {
            return _context.OrderFeedbacks.Any(e => e.Id == id);
        }
    }
}
