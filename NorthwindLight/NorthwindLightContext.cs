using Microsoft.EntityFrameworkCore;

namespace NorthwindLight;

public class NorthwindLightContext : DbContext
{
  public DbSet<Customer> Customers { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Shipment> Shipments { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderDetail> OrderDetails { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (optionsBuilder.IsConfigured)
    {
      Console.WriteLine("Is already Configured");
      return;
    }
    string connectionString = @"data source=C:\\Databases\NorthwindLite.db";
    Console.WriteLine($"Using connectionstring {connectionString}");
    optionsBuilder.UseSqlite(connectionString);
  }
}
