using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindLight;
public class Product
{
  public int Id { get; set; }
  public string Description { get; set; } = null!;
  public int Price { get; set; }
  public int Weight { get; set; }
}
