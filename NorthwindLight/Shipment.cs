using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindLight;
public class Shipment
{
  public int Id { get; set; }
  public DateTime? DeliverDate { get; set; }
  public DateTime PlanDate { get; set; }
  public int SequenceNr { get; set; }
  public int EmployeeId { get; set; }
  public Employee Employee { get; set; } = null!;
  public List<Order> Orders { get; set; } = null!;
}
