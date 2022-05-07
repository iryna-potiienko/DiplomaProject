using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;

namespace DiplomaProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly KraftWebAppContext _context;

        public OrderController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index(int? shopProfileId)
        {
            List<Order> orders =new List<Order>();
            if (shopProfileId != null)
            {
                var shop = await _context.ShopProfiles.FindAsync(shopProfileId);
                if (shop.SalesmanId == GetCurrentUserId())
                {
                    orders = await _context.Orders
                        .Include(o => o.Cart)
                        .ThenInclude(o => o.Customer)
                        .Include(o => o.ShopProfile)
                        .Where(o => o.ShopProfileId == shopProfileId)
                        //.AndInclude(o=>o.ShopProfile)
                        //.Include(o => o.Cart)
                        //.ThenInclude(o => o.ShopProfile)
                        //.Where(o=>o.Cart.ShopProfileId==shopProfileId)
                        .Include(o => o.DeliveryType)
                        .Include(o => o.ReadyStage)
                        .ToListAsync();
                }
            }
            else
            {
                orders = await _context.Orders
                    // .Include(o => o.Cart)
                    // .ThenInclude(o => o.Customer)
                    .Where(o=>o.Cart.CustomerId == GetCurrentUserId())
                    .Include(o=>o.ShopProfile)
                    //.Include(o => o.Cart)
                    //.ThenInclude(o => o.ShopProfile)
                    .Include(o => o.DeliveryType)
                    .Include(o => o.ReadyStage)
                    .ToListAsync();
                //.Include(o => o.ShopProfile);
            }

            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                //.Include(o => o.Customer)
                .Include(o => o.DeliveryType)
                .Include(o => o.ReadyStage)
                .Include(o => o.ShopProfile)
                // .Include(o => o.Cart)
                // .ThenInclude(o => o.ShopProfile)
                .Include(o => o.Cart)
                .ThenInclude(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create(int cartId)
        {
            // var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            // ViewData["CustomerId"] = user.Id;//new SelectList(_context.Users, "Id", "Id");

            ViewData["CartId"] = cartId;
            
            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Name");
            //ViewData["ReadyStageId"] = 1;//new SelectList(_context.ReadyStages, "Id", "Id");
            //ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CartId,DeliveryTypeId,DateBeReady,DateOfFixation,IsDelivered,AddressToDelivery,Comment,Price,IsPaid,ReadyStageId")] Order order)
        {
            //order.CartId = GetCart();
            if (ModelState.IsValid)
            {
                //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                //order.CustomerId = user.Id;
                var currentCart = CloseCart();
                if (currentCart!=null)
                {
                    order.ShopProfileId = currentCart.ShopProfileId;
                    order.ReadyStageId = 1;
                    order.DateOfFixation = DateTime.Today;
                    order.IsPaid = false;
                    order.IsDelivered = false;

                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            //ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Name", order.DeliveryTypeId);
            //ViewData["ReadyStageId"] = 1;//new SelectList(_context.ReadyStages, "Id", "Id", order.ReadyStageId);
            //ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id", order.ShopProfileId);
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Id", order.DeliveryTypeId);
            ViewData["ReadyStageId"] = new SelectList(_context.ReadyStages, "Id", "Id", order.ReadyStageId);
            //ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id", order.ShopProfileId);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ShopProfileId,DeliveryTypeId,DateBeReady,DateOfFixation,IsDelivered,AddressToDelivery,Comment,Price,IsPaid,ReadyStageId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            //ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Id", order.CustomerId);
            ViewData["DeliveryTypeId"] = new SelectList(_context.DeliveryTypes, "Id", "Id", order.DeliveryTypeId);
            ViewData["ReadyStageId"] = new SelectList(_context.ReadyStages, "Id", "Id", order.ReadyStageId);
            ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id", order.ShopProfileId);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                // .Include(o => o.Cart)
                // .ThenInclude(o => o.ShopProfile)
                .Include(o => o.Cart)
                .ThenInclude(o => o.Customer)
                //.Include(o => o.Customer)
                .Include(o => o.DeliveryType)
                .Include(o => o.ReadyStage)
                .Include(o => o.ShopProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
        
        public Cart CloseCart()
        {
            var sessionKey = "Cart";
            var sessionId  = HttpContext.Session.GetInt32(sessionKey);

            if (sessionId != null)
            {
                //var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

                var userId = GetCurrentUserId();

                var cart = _context.Carts
                    .Include(c => c.ProductsInOrder)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefault(c => c.Id == sessionId);
                
                if (cart.CustomerId == userId && cart.IsOpenForAddingProducts == true)
                {
                    cart.IsOpenForAddingProducts = false;
                    _context.Update(cart);
                    _context.SaveChanges();
                    
                    HttpContext.Session.Remove(sessionKey);
                    return cart;
                }
            }

            return null;
        }

        public int GetCurrentUserId()
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            return user.Id;
        }
    }
}
