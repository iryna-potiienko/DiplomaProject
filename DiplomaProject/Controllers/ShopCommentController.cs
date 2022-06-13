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
    public class ShopCommentController : Controller
    {
        private readonly KraftWebAppContext _context;

        public ShopCommentController(KraftWebAppContext context)
        {
            _context = context;
        }

        // GET: ShopComment
        public async Task<IActionResult> Index()
        {
            var shopCommentsList = _context.ShopComments
                .Include(s => s.ShopProfile)
                .Include(s => s.User);
            return View(await shopCommentsList.ToListAsync());
        }

        // GET: ShopComment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopComment = await _context.ShopComments
                .Include(s => s.ShopProfile)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopComment == null)
            {
                return NotFound();
            }

            return View(shopComment);
        }

        [Authorize]
        public IActionResult Create(int? shopProfileId)
        {
            ViewData["ShopProfileId"] = shopProfileId; //new SelectList(_context.ShopProfiles, "Id", "Id");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommentText,Date,Estimation,ShopProfileId,UserId")] ShopCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
                if (user == null)
                {
                    ModelState.AddModelError("user","User not found");
                    return NotFound();
                }

                var shopComment = new ShopComment
                {
                    CommentText = model.CommentText,
                    Estimation = model.Estimation,
                    ShopProfileId = model.ShopProfileId,
                    UserId = user.Id,
                    Date = DateTime.Now
                };

                _context.Add(shopComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), nameof(ShopProfile), new {id = shopComment.ShopProfileId});
            }
            //ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id", shopComment.ShopProfileId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shopComment.UserId);
            return View(model);
        }

        // GET: ShopComment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopComment = await _context.ShopComments.FindAsync(id);
            if (shopComment == null)
            {
                return NotFound();
            }

            var model = new ShopCommentViewModel
            {
                CommentText = shopComment.CommentText,
                Estimation = shopComment.Estimation,
                //ShopProfileId = shopComment.ShopProfileId
            };
            
            //ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id", shopComment.ShopProfileId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shopComment.UserId);
            return View(model);
        }

        // POST: ShopComment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentText,Date,Estimation,ShopProfileId")] ShopCommentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ShopComment shopComment;
                try
                {
                    shopComment = await _context.ShopComments.FindAsync(id);

                    shopComment.CommentText = model.CommentText;
                    shopComment.Estimation = model.Estimation;
                    //shopComment.ShopProfileId = model.ShopProfileId;
                    //shopComment.UserId = user.Id;
                    shopComment.Date = DateTime.Now;
                    _context.Update(shopComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopCommentExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), nameof(ShopProfile), new {id = shopComment.ShopProfileId});
            }
            //ViewData["ShopProfileId"] = new SelectList(_context.ShopProfiles, "Id", "Id", shopComment.ShopProfileId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shopComment.UserId);
            return View(model);
        }

        // GET: ShopComment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopComment = await _context.ShopComments
                .Include(s => s.ShopProfile)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopComment == null)
            {
                return NotFound();
            }

            return View(shopComment);
        }

        // POST: ShopComment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopComment = await _context.ShopComments.FindAsync(id);
            _context.ShopComments.Remove(shopComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), nameof(ShopProfile), new {id = shopComment.ShopProfileId});
        }

        private bool ShopCommentExists(int id)
        {
            return _context.ShopComments.Any(e => e.Id == id);
        }
    }
}
