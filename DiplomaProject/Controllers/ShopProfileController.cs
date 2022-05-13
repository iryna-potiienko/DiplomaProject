using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;
using DiplomaProject.ViewModels;

namespace DiplomaProject.Controllers
{
    public class ShopProfileController : Controller
    {
        private readonly KraftWebAppContext _context;

        public ShopProfileController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: ShopProfile
        public async Task<IActionResult> Index(int? salesmanId)
        {
            Task<List<ShopProfile>> shopProfiles;
            if (salesmanId != null)
            {
                shopProfiles = _context.ShopProfiles
                    .Where(s => s.SalesmanId == salesmanId).ToListAsync();
            }
            else
            {
                shopProfiles = _context.ShopProfiles
                    .Include(s => s.Salesman).ToListAsync();
            }

            return View(await shopProfiles);
        }

        // GET: ShopProfile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopProfile = await _context.ShopProfiles
                .Include(s => s.Salesman)
                .Include(s=>s.Products)
                .Include(s=>s.Orders)
                .ThenInclude(o=>o.OrderFeedback)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopProfile == null)
            {
                return NotFound();
            }

            //ViewBag.ShopProfileId = shopProfile.Id;
            ViewBag.ProductsCount = shopProfile.Products.Count;
            ViewBag.OrdersCount = shopProfile.Orders.Count;
            //_context.Orders.Count(o => o.ShopProfileId == (int) id);
            ViewBag.OrdersUserGetCount = //_context.Orders
                shopProfile.Orders
                //.Where(o => o.ShopProfileId == (int) id)
                .Count(o => o.ReadyStageId == 9);

            var shopOrders = _context.Orders
                .Include(o => o.OrderFeedback)
                .Where(o => o.ShopProfileId == (int) id);
            ViewBag.OrdersInTimeCount = //_context.Orders
            //     .Include(o => o.OrderFeedback)
            //     .Where(o => o.ShopProfileId == (int) id)
            shopOrders.Count(o => o.OrderFeedback.IsInTime == true);
            ViewBag.OrdersSuccesfulCount = //_context.Orders
            //     .Include(o => o.OrderFeedback)
            //     .Where(o => o.ShopProfileId == (int) id)
                shopOrders.Count(o => o.OrderFeedback.IsEverythingOkay == true);
            
            return View(shopProfile);
        }

        // GET: ShopProfile/Create
        public IActionResult Create()
        {
            //ViewData["SalesmanId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ShopProfile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City,Address,LogoPhoto,Description,Contacts")] ShopProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                var shopProfile = new ShopProfile
                {
                    Name = model.Name,
                    Address = model.Address,
                    City = model.City,
                    Contacts = model.Contacts,
                    Description = model.Description,
                    LogoPhoto = model.LogoPhoto,
                    Latitude = 0,
                    Longitude = 0,
                    IsVerified = false,
                    SalesmanId = user.Id
                };
                
                _context.Add(shopProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["SalesmanId"] = new SelectList(_context.Users, "Id", "Id", shopProfile.SalesmanId);
            return View(model);
        }

        // GET: ShopProfile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopProfile = await _context.ShopProfiles.FindAsync(id);
            if (shopProfile == null)
            {
                return NotFound();
            }
            ViewData["SalesmanId"] = new SelectList(_context.Users, "Id", "Id", shopProfile.SalesmanId);
            return View(shopProfile);
        }

        // POST: ShopProfile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SalesmanId,IsVerified,City,Address,Latitude,Longitude,LogoPhoto,Description,Contacts")] ShopProfile shopProfile)
        {
            if (id != shopProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopProfileExists(shopProfile.Id))
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
            ViewData["SalesmanId"] = new SelectList(_context.Users, "Id", "Id", shopProfile.SalesmanId);
            return View(shopProfile);
        }

        // GET: ShopProfile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopProfile = await _context.ShopProfiles
                .Include(s => s.Salesman)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopProfile == null)
            {
                return NotFound();
            }

            return View(shopProfile);
        }

        // POST: ShopProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopProfile = await _context.ShopProfiles.FindAsync(id);
            _context.ShopProfiles.Remove(shopProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopProfileExists(int id)
        {
            return _context.ShopProfiles.Any(e => e.Id == id);
        }

        public async Task<IActionResult> VerifyShopProfile(int id)
        {
            var shopProfile = await _context.ShopProfiles.FindAsync(id);
            shopProfile.IsVerified = true;
            
            _context.ShopProfiles.Update(shopProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = shopProfile.Id});
        }
    }
}
