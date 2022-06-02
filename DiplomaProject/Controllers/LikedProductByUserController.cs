using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;

namespace DiplomaProject.Controllers
{
    public class LikedProductByUserController : Controller
    {
        private readonly KraftWebAppContext _context;
        private readonly IAccountRepository _accountRepository;

        public LikedProductByUserController(KraftWebAppContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index(int? userId, string currentUserEmail)
        {
            if (currentUserEmail != null)
            {
                var user = _accountRepository.GetCurrentUser(currentUserEmail);
                userId = user.Id;
            }

            if (userId != null)
            {
                var likedProductsByUser = _context.LikedProductsByUsers
                    .Where(l => l.UserId == userId)
                    .Include(l => l.Product)
                    .ThenInclude(p => p.ShopProfile);
                return View(await likedProductsByUser.ToListAsync());
            }

            var likedProducts = _context.LikedProductsByUsers
                .Include(l => l.Product)
                .Include(l => l.User);
            return View(await likedProducts.ToListAsync());
        }

        public async Task<IActionResult> Create(int productId, string username)
        {
            var user = _accountRepository.GetCurrentUser(username);
            var likedProductsByUsers = new LikedProductsByUsers
            {
                ProductId = productId,
                UserId = user.Id
            };
                
            _context.Add(likedProductsByUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {userId = user.Id});
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var likedProductsByUsers = await _context.LikedProductsByUsers.FindAsync(id);
            if (likedProductsByUsers == null)
            {
                return NotFound();
            }
            _context.LikedProductsByUsers.Remove(likedProductsByUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {userId = likedProductsByUsers.UserId});
        }

        private bool LikedProductsByUsersExists(int id)
        {
            return _context.LikedProductsByUsers.Any(e => e.Id == id);
        }
    }
}
