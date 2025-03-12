using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindLight;
public class Order
{
  public int Id { get; set; }
  public string Description { get; set; } = null!;
  public DateTime? OrderDate { get; set; }
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
  public int? ShipmentId { get; set; }
  public Shipment? Shipment { get; set; }
}
