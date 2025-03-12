using NorthwindLight;

using System.Net.Http.Headers;

namespace NorthwindLightWpf;
internal class DgRow
{
  public required string EmployeeName { get; set; }
  public required string Order { get; set; }
  public required string CustomerName { get; set; }
  public int SequenceNr { get; set; }
  public DateTime? DeliverDate { get; set; }
  public required DateTime PlanDate { get; set; }

  internal static DgRow FromShipment(Shipment shipment)
  {
    var customerName = shipment.Orders.Single(x => x.ShipmentId == shipment.Id).Customer.Name;
    var order = shipment.Orders.Single(x => x.ShipmentId == shipment.Id).Description;
    return new DgRow()
    {
      CustomerName = customerName,
      DeliverDate = shipment.DeliverDate,
      EmployeeName = shipment.Employee.Name,
      Order = order,
      PlanDate = shipment.PlanDate,
      SequenceNr = shipment.SequenceNr,
    };
  }
}
