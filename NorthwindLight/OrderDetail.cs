using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindLight;
public class OrderDetail
{
  public int Id { get; set; }
  public int Amount { get; set; }
  public int OrderId { get; set; }
  public Order Order { get; set; } = null!;
  public int ProductId { get; set; }
  public Product Product { get; set; } = null!;

}
