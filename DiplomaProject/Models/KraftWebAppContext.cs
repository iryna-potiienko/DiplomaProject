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
    public DbSet<Cart> Carts { get; set; }

    public DbSet<DeliveryType> DeliveryTypes { get; set; }
    public DbSet<ReadyStage> ReadyStages { get; set; }
    public DbSet<Role> Roles { get; set; }

    public KraftWebAppContext(DbContextOptions<KraftWebAppContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        string adminRoleName = "admin";
        string userRoleName = "user";
 
        string adminEmail = "admin@mail.ru";
        string adminPassword = "123456";
 
        // добавляем роли
        Role adminRole = new Role { Id = 1, Name = adminRoleName };
        Role userRole = new Role { Id = 2, Name = userRoleName };
        Role salesmanRole = new Role { Id = 3, Name = "salesman" };
        User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
 
        modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole, salesmanRole });
        modelBuilder.Entity<User>().HasData( new User[] { adminUser });
        base.OnModelCreating(modelBuilder);
    }
}