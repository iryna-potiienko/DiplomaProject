using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;
using DiplomaProject.ViewModels;

namespace DiplomaProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly KraftWebAppContext _context;

        public ProductController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(int? shopProfileId, int? categoryId, int? subcategoryId)
        {
            Task<List<Product>> products;
            if(shopProfileId!=null)
                products = _context.Products.Where(p => p.ShopProfileId == shopProfileId).ToListAsync();

            else if (categoryId != null)
            {
                // var subcategories = _context.Subcategories
                //     .Where(s => s.CategoryId == categoryId)
                //     .ToList();

                products = _context.Subcategories
                    .Where(s => s.CategoryId == categoryId)
                    //.ToList()
                    .SelectMany(subcategory => _context.Products
                        .Include(p => p.ShopProfile)
                        .Where(p => p.SubcategoryId == subcategory.Id))
                    //.ToList())
                    .ToListAsync();
                ViewBag.CategoryId = categoryId;
            }
            else if (subcategoryId != null)
            {
                products = _context.Products
                    .Include(p => p.ShopProfile)
                    .Where(p => p.SubcategoryId == subcategoryId)
                    .ToListAsync();
            }
            else
            {
                products = _context.Products.Include(p => p.ShopProfile).ToListAsync();
            }
            ViewBag.ShopProfileId = shopProfileId;
            return View(await products);
        }

        public async Task<IActionResult> GetByName(string name)
        {
            Task<List<Product>> products;
            if(name!=null)
                products = _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();
            else
            {
                products = _context.Products.Include(p => p.ShopProfile).ToListAsync();
            }
            //ViewBag.ShopProfileId = shopProfileId;
            return View("Index", await products);
        }
        
        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ShopProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create(int? shopProfileId)
        {
            ViewData["ShopProfileId"] = shopProfileId;
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShopProfileId,SubcategoryId,Description,Photo,Composition,Price")] ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] photo = new byte[] { };
                await using (var memoryStream = new MemoryStream())
                {
                    await model.Photo.CopyToAsync(memoryStream);
                
                    // Upload the file if less than 16 MB
                    if (memoryStream.Length < 16777216)
                    {
                        photo = memoryStream.ToArray();
                        // var file = new Product()
                        // {
                        //     Photo = memoryStream.ToArray()
                        // };
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }

                var product = new Product
                {
                    Name = model.Name,
                    Composition = model.Composition,
                    Description = model.Description,
                    Price = model.Price,
                    ShopProfileId = model.ShopProfileId,
                    SubcategoryId = model.SubcategoryId,
                    Photo = photo
                };
                
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //ViewData["ShopProfileId"] = shopProfileId;//new SelectList(_context.ShopProfiles, "Id", "Id", product.ShopProfileId);
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "Id", "Name", model.SubcategoryId);
            return View(model);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            
            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Composition = product.Composition,
                Description = product.Description,
                Price = product.Price,
                ShopProfileId = product.ShopProfileId,
                SubcategoryId = product.SubcategoryId,
                //Photo = photo
            };
            
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "Id", "Name", product.SubcategoryId);
            return View(model);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShopProfileId,Description,Photo,Composition,Price,SubcategoryId")] ProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(id);
                    await using (var memoryStream = new MemoryStream())
                    {
                        await model.Photo.CopyToAsync(memoryStream);

                        // Upload the file if less than 16 MB
                        if (memoryStream.Length < 16777216)
                        {
                            product.Photo = memoryStream.ToArray();
                        }
                    }

                    product.Name = model.Name;
                    product.Composition = model.Composition;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.ShopProfileId = model.ShopProfileId;
                    product.SubcategoryId = model.SubcategoryId;
                    
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.Id))
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
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "Id", "Name", model.SubcategoryId);
            return View(model);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ShopProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
