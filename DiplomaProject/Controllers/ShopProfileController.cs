using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomaProject.Models;
using DiplomaProject.Repositories;
using DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Http;

namespace DiplomaProject.Controllers
{
    public class ShopProfileController : Controller
    {
        //private readonly KraftWebAppContext _context;
        private readonly IShopProfileRepository _shopProfileRepository;
        private readonly IAccountRepository _accountRepository;

        public ShopProfileController(IShopProfileRepository shopProfileRepository,
            IAccountRepository accountRepository)
        {
            //_context = context;
            _shopProfileRepository = shopProfileRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index(int? salesmanId)
        {
            var shopProfiles = _shopProfileRepository.GetShopProfiles(salesmanId);
            return View(await shopProfiles);
        }
        
        public async Task<IActionResult> GetByName(string name)
        {
            var shopProfiles = _shopProfileRepository.GetShopProfilesByName(name);
            return View("Index", await shopProfiles);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopProfile = await _shopProfileRepository.GetShopProfileDetailedById((int)id);
            if (shopProfile == null)
            {
                return NotFound();
            }

            return View(shopProfile);
        }
        public IActionResult Create(string username)
        {
            //var user = _accountRepository.GetCurrentUser(username);
            //ViewData["Userid"] = user.Id;
            ViewData["Username"] = username;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Name,City,Address,LogoPhoto,Description,Contacts")] ShopProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _accountRepository.GetCurrentUser(model.Username);//(User.Identity.Name);
                try
                {
                    await _shopProfileRepository.CreateShopProfile(model,user.Id);// model.Userid); //user.Id);
                }
                catch (FileNotFoundException)
                {
                    return NotFound();
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopProfile = await _shopProfileRepository.GetShopProfileById((int)id);
            if (shopProfile == null)
            {
                return NotFound();
            }

            var model = new ShopProfileViewModel
            {
                Id=shopProfile.Id,
                Name = shopProfile.Name,
                Address = shopProfile.Address,
                City = shopProfile.City,
                Contacts = shopProfile.Contacts,
                Description = shopProfile.Description
            };
            
            return View(model);
        }

        // POST: ShopProfile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SalesmanId,IsVerified,City,Address,Latitude,Longitude,LogoPhoto,Description,Contacts")] ShopProfileViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _shopProfileRepository.UpdateShopProfile(id, model);
                }
                catch (FileNotFoundException)
                {
                    return NotFound();
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }

        // GET: ShopProfile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopProfile = await _shopProfileRepository.GetShopProfileWithSalesmanById((int)id);
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
            await _shopProfileRepository.DeleteShopProfile(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> VerifyShopProfile(int id)
        {
            var shopProfile = await _shopProfileRepository.VerifyShopProfile(id);
            if (shopProfile == null)
            {
                return NotFound();
            }
            
            return RedirectToAction(nameof(Details), new {id = shopProfile.Id});
        }
        
        public JsonResult GetData()
        {
            //var client = new MapsAPIClient("AIzaSyBfckBchOpn-lM4oJ9V9nBDBZmmlousIRQ");

            // Geocoding an address
            //var geocodeResult = client.Geocoding.Geocode("1600 Amphitheatre Parkway, Mountain View, CA");

            var brokerAdresses = new List<ShopProfileOnMapViewModel>();
            var shopProfiles = _shopProfileRepository.GetShopProfiles(null).Result;
            foreach(var b in shopProfiles)
            {
                if (b.Address == null) continue;
                brokerAdresses.Add(new ShopProfileOnMapViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    City = b.City,
                    Address = b.Address,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude
                });
            }

            return Json(brokerAdresses);
        }
    }
}
