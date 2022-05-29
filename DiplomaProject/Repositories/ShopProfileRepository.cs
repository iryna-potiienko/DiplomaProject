using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DiplomaProject.IRepositories;
using DiplomaProject.Models;
using DiplomaProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.Repositories;

public class ShopProfileRepository: IShopProfileRepository
{
    private readonly KraftWebAppContext _context;

    public ShopProfileRepository(KraftWebAppContext context)
    {
        _context = context;
    }

    public Task<List<ShopProfile>> GetShopProfiles(int? salesmanId)
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

        return shopProfiles;
    }
    
    public ValueTask<ShopProfile?> GetShopProfileById(int id)
    {
        return _context.ShopProfiles.FindAsync(id);
    }
    
    public Task<ShopProfile> GetShopProfileWithSalesmanById(int id)
    {
        return _context.ShopProfiles
            .Include(s => s.Salesman)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
    
    public Task<List<ShopProfile>> GetShopProfilesByName(string name)
    {
        Task<List<ShopProfile>> shopProfiles;
        if (name != null)
        {
            shopProfiles = _context.ShopProfiles
                .Where(s => s.Name.Contains(name)).ToListAsync();
        }
        else
        {
            shopProfiles = _context.ShopProfiles
                .Include(s => s.Salesman).ToListAsync();
        }

        return shopProfiles;
    }
    
    public Task<ShopProfile> GetShopProfileDetailedById(int id)
    {
        var shopProfile = _context.ShopProfiles
            .Include(s => s.Salesman)
            .Include(s=>s.Products)
            .Include(s=>s.Orders)
            .ThenInclude(o=>o.OrderFeedback)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        return shopProfile;
    }

    public async Task CreateShopProfile(ShopProfileViewModel model, int userId)
    {
        // byte[] logoPhoto;
        // await using (var memoryStream = new MemoryStream())
        // {
        //     await model.LogoPhoto.CopyToAsync(memoryStream);
        //
        //     // Upload the file if less than 16 MB
        //     if (memoryStream.Length < 16777216)
        //     {
        //         logoPhoto = memoryStream.ToArray();
        //     }
        //     else
        //     {
        //         throw new IOException("Файл завеликий");
        //     }
        // }

        var logoPhoto = await AddLogoPhoto(model.LogoPhoto);
        
        var shopProfile = new ShopProfile
        {
            Name = model.Name,
            Address = model.Address,
            City = model.City,
            Contacts = model.Contacts,
            Description = model.Description,
            LogoPhoto = logoPhoto,
            Latitude = 0,
            Longitude = 0,
            IsVerified = false,
            SalesmanId = userId
        };
                
        _context.Add(shopProfile);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShopProfile(int id, ShopProfileViewModel model)
    {
        try
        {
            var shopProfile = await _context.ShopProfiles.FindAsync(id);
            if (shopProfile == null)
                //return NotFound();
                throw new FileNotFoundException();

            // await using (var memoryStream = new MemoryStream())
            // {
            //     await model.LogoPhoto.CopyToAsync(memoryStream);
            //
            //     // Upload the file if less than 16 MB
            //     if (memoryStream.Length < 16777216)
            //     {
            //         shopProfile.LogoPhoto = memoryStream.ToArray();
            //     }
            // }

            shopProfile.LogoPhoto = await AddLogoPhoto(model.LogoPhoto);
            shopProfile.Name = model.Name;
            shopProfile.Contacts = model.Contacts;
            shopProfile.Description = model.Description;
            shopProfile.City = model.City;
            shopProfile.Address = model.Address;
            // shopProfile.Latitude = 0;
            // shopProfile.Longitude = 0;
                    
            _context.Update(shopProfile);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ShopProfileExists(model.Id))
            {
                throw new FileNotFoundException();
            }
            else
            {
                throw;
            }
        }
    }

    public async Task DeleteShopProfile(int id)
    {
        var shopProfile = await _context.ShopProfiles.FindAsync(id);
        if (shopProfile == null)
            throw new FileNotFoundException();
        
        _context.ShopProfiles.Remove(shopProfile);
        await _context.SaveChangesAsync();
    }
    
    public async Task<ShopProfile> VerifyShopProfile(int id)
    {
        var shopProfile = await GetShopProfileById(id);//_context.ShopProfiles.FindAsync(id);
        if (shopProfile == null)
            return null;
        
        shopProfile.IsVerified = true;
        _context.ShopProfiles.Update(shopProfile);
        await _context.SaveChangesAsync();
        return shopProfile;
        //return RedirectToAction(nameof(Details), new {id = shopProfile.Id});
    }
    
    private bool ShopProfileExists(int id)
    {
        return _context.ShopProfiles.Any(e => e.Id == id);
    }
    private async Task<byte[]> AddLogoPhoto(IFormFile modelLogoPhoto)
    {
        byte[] logoPhoto;
        using (var memoryStream = new MemoryStream())
        {
            await modelLogoPhoto.CopyToAsync(memoryStream);

            // Upload the file if less than 16 MB
            if (memoryStream.Length < 16777216)
            {
                logoPhoto = memoryStream.ToArray();
            }
            else
            {
                throw new IOException("Файл завеликий");
            }
        }
        return logoPhoto;
    }
}