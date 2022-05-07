using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;
using Microsoft.AspNetCore.Http;

namespace DiplomaProject.Controllers
{
    public class ProductInOrderController : Controller
    {
        private readonly KraftWebAppContext _context;

        public ProductInOrderController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: ProductInOrder
        public async Task<IActionResult> Index()
        {
            Task<List<ProductInOrder>> productsInOrder;
            var cart = await GetCart();//HttpContext.Session.GetInt32("Cart");//GetCart().Id;
            // if (cartId != null)
            // {
                var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                productsInOrder = _context.ProductsInOrder
                    // .Include(p => p.Cart)
                    // .ThenInclude(c=>c.CustomerId==user.Id)
                    .Where(p => p.CartId == cart.Id && p.Cart.CustomerId == user.Id)
                    .Include(p => p.Product)
                    .ToListAsync();

                ViewBag.CartId = cart.Id;
            // }
            // else
            // {
            //     productsInOrder = _context.ProductsInOrder
            //         .Include(p => p.Cart)
            //         .Include(p => p.Product)
            //         .ToListAsync();
            // }

            return View(await productsInOrder);
        }

        // GET: ProductInOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInOrder = await _context.ProductsInOrder
                .Include(p => p.Cart)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productInOrder == null)
            {
                return NotFound();
            }

            return View(productInOrder);
        }

        // GET: ProductInOrder/Create
        public IActionResult Create(int productId, int shopProfileId)
        {
            //ViewData["CartId"] = GetCart().Id;//new SelectList(_context.Orders, "Id", "Id");
            ViewData["ProductId"] = productId; //new SelectList(_context.Products, "Id", "Id");
            ViewBag.ShopProfileId = shopProfileId;
            return View();
        }

        // POST: ProductInOrder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,ShopProfileId,CartId,Amount,Comment,FinalPrice,FinalDescription")] ProductInOrder productInOrder)
        {
            //var product = await _context.Products.FindAsync(productInOrder.ProductId);
            var cart = await GetCart();
            productInOrder.CartId = cart.Id;
            
            if (ModelState.IsValid)
            {
                // if (cart.ProductsInOrder.Count != 0)
                // {
                //     var firstProductInOrder = cart.ProductsInOrder.First();
                //     var shopProfileId = (int) ViewBag.ShopProfileId;
                //
                //     if (firstProductInOrder.Product.ShopProfileId != shopProfileId)
                //     {
                //         return View(productInOrder);
                //     }
                // }

                _context.Add(productInOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", productInOrder.OrderId);
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productInOrder.ProductId);
            return View(productInOrder);
        }

        // GET: ProductInOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInOrder = await _context.ProductsInOrder.FindAsync(id);
            if (productInOrder == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Orders, "Id", "Id", productInOrder.ProductId);
            ViewData["CartId"] = new SelectList(_context.Products, "Id", "Id", productInOrder.CartId);
            return View(productInOrder);
        }

        // POST: ProductInOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,CartId,Amount,Comment,FinalPrice,FinalDescription")] ProductInOrder productInOrder)
        {
            if (id != productInOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInOrderExists(productInOrder.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Orders, "Id", "Id", productInOrder.ProductId);
            ViewData["CartId"] = new SelectList(_context.Products, "Id", "Id", productInOrder.CartId);
            return View(productInOrder);
        }

        // GET: ProductInOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInOrder = await _context.ProductsInOrder
                .Include(p => p.Cart)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productInOrder == null)
            {
                return NotFound();
            }

            return View(productInOrder);
        }

        // POST: ProductInOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInOrder = await _context.ProductsInOrder.FindAsync(id);
            _context.ProductsInOrder.Remove(productInOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInOrderExists(int id)
        {
            return _context.ProductsInOrder.Any(e => e.Id == id);
        }
        
        public async Task<Cart> GetCart()
        {
            var sessionKey = "Cart";
            var sessionId  = HttpContext.Session.GetInt32(sessionKey);
            Cart cart;
            
            if (sessionId == null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                cart = _context.Carts
                    .FirstOrDefault(o => o.CustomerId == user.Id && o.IsOpenForAddingProducts == true);

                if (cart == null)
                {
                    //var product = await _context.Products.FindAsync(productId);
                    
                    cart = new Cart
                    {
                        //ShopProfileId = product.ShopProfileId,
                        CustomerId = user.Id,
                        IsOpenForAddingProducts = true
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }

                sessionId = cart.Id;
                HttpContext.Session.SetInt32(sessionKey, (int) sessionId);
            }
            else
            {
                cart = await _context.Carts
                    .Include(c => c.ProductsInOrder)
                    .ThenInclude(p=>p.Product)
                    .FirstOrDefaultAsync(c => c.Id == sessionId);
            }
            return cart;
        }
    }
}
