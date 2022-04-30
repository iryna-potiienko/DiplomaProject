using Microsoft.EntityFrameworkCore;

namespace DiplomaProject.Models;

public class KraftWebAppContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ShopProfile> ShopProfiles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderFeedback> OrderFeedbacks { get; set; }
    public DbSet<ProductInOrder> ProductsInOrder { get; set; }
    public DbSet<LikedProductsByUsers> LikedProductsByUsers { get; set; }
    public DbSet<ShopComment> ShopComments { get; set; }
    public DbSet<ProductComment> ProductComments { get; set; }
    
    public DbSet<DeliveryType> DeliveryTypes { get; set; }
    public DbSet<ReadyStage> ReadyStages { get; set; }
    public DbSet<Role> Roles { get; set; }

    public KraftWebAppContext(DbContextOptions<KraftWebAppContext> options) : base(options)
    {
        
    }
}