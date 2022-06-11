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
        public const string SessionKey = "Cart";

        public ProductInOrderController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: ProductInOrder
        public async Task<IActionResult> Index()
        {
            List<ProductInOrder> productsInOrder;
            
            var sessionId = HttpContext.Session.GetInt32(SessionKey);
            var cart = GetCurrentCart(sessionId); //await GetCart();
            if (cart != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                var productsList = _context.ProductsInOrder
                    .Where(p => p.CartId == cart.Id && p.Cart.CustomerId == user.Id)
                    .Include(p => p.Product)
                    .ToListAsync();

                ViewBag.CartId = cart.Id;
                productsInOrder = await productsList;
            }
            else
            {
                productsInOrder = new List<ProductInOrder>();
                // productsInOrder = _context.ProductsInOrder
                //     .Include(p => p.Cart)
                //     .Include(p => p.Product)
                //     .ToListAsync();
            }

            return View(productsInOrder);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,ShopProfileId,CartId,Amount,Comment,FinalPrice,FinalDescription")] ProductInOrder productInOrder)
        {
            //var product = await _context.Products.FindAsync(productInOrder.ProductId);
            
            if (!ModelState.IsValid) return View(productInOrder);
            
            var cart = await GetCart();
            productInOrder.CartId = cart.Id;
            var product = _context.Products.FirstOrDefault(p => p.Id == productInOrder.ProductId);

            if (cart.ProductsInOrder.Count == 0)
            {
                cart.ShopProfileId = product.ShopProfileId;
                _context.Update(cart);
                //_context.SaveChanges();
            }
            else
            {
                if (cart.ShopProfileId != product.ShopProfileId)
                {
                    //ModelState.AddModelError("CartShopProfile","You made order in shop"+cart.ShopProfileId);
                    ViewBag.CartShopProfileError = "You made order in shop" + cart.ShopProfileId;
                    return View(productInOrder);
                }
                    
            }

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
            var sessionId  = HttpContext.Session.GetInt32(SessionKey);
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
                    await _context.SaveChangesAsync();
                }

                sessionId = cart.Id;
                HttpContext.Session.SetInt32(SessionKey, (int) sessionId);
            }
            else
            {
                // cart = await _context.Carts
                //     .Include(c => c.ProductsInOrder)
                //     .ThenInclude(p=>p.Product)
                //     .FirstOrDefaultAsync(c => c.Id == sessionId);
                cart = GetCurrentCart(sessionId);
            }
            return cart;
        }
        
        public Cart GetCurrentCart(int? sessionId)
        {
            //var sessionId = HttpContext.Session.GetInt32(SessionKey);
            if (sessionId == null) return null;
            
            var cart = _context.Carts
                .Include(c => c.ProductsInOrder)
                .ThenInclude(p => p.Product)
                .FirstOrDefault(c => c.Id == sessionId);
            return cart;
        }
    }
}
