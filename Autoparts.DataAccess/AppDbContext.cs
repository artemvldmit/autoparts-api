using Autoparts.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autoparts.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Order>().Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, Name = "Admin", Email = "admin@autoparts.ru",
                Phone = "+7 (000) 000-00-00", Role = "Admin",
                PasswordHash = "admin123",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Масляный фильтр Mann", Price = 450, Category = "filters", ImageUrl = "https://images.unsplash.com/photo-1600712242805-5f78671b24da?w=300&h=200&fit=crop", Stock = 50 },
            new Product { Id = 2, Name = "Воздушный фильтр Bosch", Price = 380, Category = "filters", ImageUrl = "https://images.unsplash.com/photo-1625047509168-a7026f36de04?w=300&h=200&fit=crop", Stock = 40 },
            new Product { Id = 3, Name = "Тормозные колодки Brembo", Price = 2800, Category = "brakes", ImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=300&h=200&fit=crop", Stock = 20 },
            new Product { Id = 4, Name = "Тормозной диск TRW", Price = 3200, Category = "brakes", ImageUrl = "https://images.unsplash.com/photo-1568605117036-5fe5e7bab0b7?w=300&h=200&fit=crop", Stock = 15 },
            new Product { Id = 5, Name = "Амортизатор KYB передний", Price = 4500, Category = "suspension", ImageUrl = "https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?w=300&h=200&fit=crop", Stock = 10 },
            new Product { Id = 6, Name = "Пружина подвески Monroe", Price = 2100, Category = "suspension", ImageUrl = "https://images.unsplash.com/photo-1503376780353-7e6692767b70?w=300&h=200&fit=crop", Stock = 12 },
            new Product { Id = 7, Name = "Свечи зажигания NGK (4 шт)", Price = 1200, Category = "engine", ImageUrl = "https://images.unsplash.com/photo-1486262715619-67b85e0b08d3?w=300&h=200&fit=crop", Stock = 60 },
            new Product { Id = 8, Name = "Ремень ГРМ Gates", Price = 1800, Category = "engine", ImageUrl = "https://images.unsplash.com/photo-1617788138017-80ad40651399?w=300&h=200&fit=crop", Stock = 25 },
            new Product { Id = 9, Name = "Моторное масло Castrol 5W-40 (4л)", Price = 2600, Category = "engine", ImageUrl = "https://images.unsplash.com/photo-1599305090598-fe179d501227?w=300&h=200&fit=crop", Stock = 30 },
            new Product { Id = 10, Name = "Аккумулятор Varta 60Ah", Price = 8500, Category = "electrical", ImageUrl = "https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?w=300&h=200&fit=crop", Stock = 8 },
            new Product { Id = 11, Name = "Генератор Bosch", Price = 12000, Category = "electrical", ImageUrl = "https://images.unsplash.com/photo-1487754180451-c456f719a1fc?w=300&h=200&fit=crop", Stock = 5 },
            new Product { Id = 12, Name = "Лампа H7 Philips (2 шт)", Price = 650, Category = "electrical", ImageUrl = "https://images.unsplash.com/photo-1544620347-c4fd4a3d5957?w=300&h=200&fit=crop", Stock = 45 }
        );
    }
}
