using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DiplomaProject.Models;
using DiplomaProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.IRepositories;

public interface IShopProfileRepository
{
    public Task<List<ShopProfile>> GetShopProfiles(int? salesmanId);
    public ValueTask<ShopProfile?> GetShopProfileById(int id);
    public Task<ShopProfile> GetShopProfileWithSalesmanById(int id);
    public Task<List<ShopProfile>> GetShopProfilesByName(string name);
    public Task<ShopProfile> GetShopProfileDetailedById(int id);
    public Task<bool> CreateShopProfile(ShopProfileViewModel model, int userId);
    public Task<ShopProfile> UpdateShopProfile(int id, ShopProfileViewModel model);
    public Task<bool> DeleteShopProfile(int id);
    public Task<ShopProfile> VerifyShopProfile(int id);
}