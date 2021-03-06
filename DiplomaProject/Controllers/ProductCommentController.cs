using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;
using DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaProject.Controllers
{
    public class ProductCommentController : Controller
    {
        private readonly KraftWebAppContext _context;

        public ProductCommentController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: ProductComment
        public async Task<IActionResult> Index()
        {
            var kraftWebAppContext = _context.ProductComments.Include(p => p.Product).Include(p => p.User);
            return View(await kraftWebAppContext.ToListAsync());
        }

        // GET: ProductComment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productComment = await _context.ProductComments
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productComment == null)
            {
                return NotFound();
            }

            return View(productComment);
        }

        [Authorize]
        public IActionResult Create(int productId)
        {
            ViewData["ProductId"] = productId;//new SelectList(_context.Products, "Id", "Id");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,UserId,CommentText,Date,Estimation")] ProductCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                if (user == null)
                {
                    ModelState.AddModelError("user","User not found");
                    return NotFound();
                }
                
                var productComment = new ProductComment
                {
                    CommentText = model.CommentText,
                    Estimation = model.Estimation,
                    ProductId = model.ProductId,
                    UserId = user.Id,
                    Date = DateTime.Now
                };
                
                _context.Add(productComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), nameof(Product), new {id = productComment.ProductId});
            }
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productComment.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", productComment.UserId);
            return View(model);
        }

        // GET: ProductComment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productComment = await _context.ProductComments.FindAsync(id);
            if (productComment == null)
            {
                return NotFound();
            }
            
            var model = new ProductCommentViewModel
            {
                CommentText = productComment.CommentText,
                Estimation = productComment.Estimation,
                //ShopProfileId = shopComment.ShopProfileId
            };
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productComment.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", productComment.UserId);
            return View(model);
        }

        // POST: ProductComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserId,CommentText,Date,Estimation")] ProductCommentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ProductComment productComment;
                try
                {
                    productComment = await _context.ProductComments.FindAsync(id);

                    productComment.CommentText = model.CommentText;
                    productComment.Estimation = model.Estimation;
                    productComment.Date = DateTime.Now;
                    
                    _context.Update(productComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCommentExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), nameof(Product), new {id = productComment.ProductId});
            }
            //ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productComment.ProductId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", productComment.UserId);
            return View(model);
        }

        // GET: ProductComment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productComment = await _context.ProductComments
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productComment == null)
            {
                return NotFound();
            }

            return View(productComment);
        }

        // POST: ProductComment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productComment = await _context.ProductComments.FindAsync(id);
            _context.ProductComments.Remove(productComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), nameof(Product), new {id = productComment.ProductId});

        }

        private bool ProductCommentExists(int id)
        {
            return _context.ProductComments.Any(e => e.Id == id);
        }
    }
}
