using Microsoft.EntityFrameworkCore;

namespace NorthwindLight;

public class NorthwindLightContext : DbContext
{
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
