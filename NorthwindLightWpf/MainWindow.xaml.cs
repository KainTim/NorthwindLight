using NorthwindLight;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NorthwindLightWpf;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  NorthwindLightContext db;
  private int _sequence = 1;
  public MainWindow()
  {
    InitializeComponent();
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    db = new NorthwindLightContext();
    db.Database.EnsureDeleted(); Console.WriteLine("Deleted");
    db.Database.EnsureCreated(); Console.WriteLine("Created");
    SeedData();
    DisplayProducts();
    DisplayTreeView();
    DisplayFilteredEmployees();
    dtpDeliveryDate.SelectedDate = DateTime.Today;
  }

  private void DisplayFilteredEmployees(){
    lsbEmployees.ItemsSource = db.Employees.Where(x=>x.LastName.ToLower().Contains(txtEmployees.Text.ToLower())).ToList();
  }

  private void DisplayTreeView() {
    trvCustomers.Items.Clear();
    var root = new TreeViewItem()
    {
      Header = "Kunden",
    };
    trvCustomers.Items.Add(root);
    foreach (var customer in db.Customers)
    {
      var customerItem = new TreeViewItem()
      {
        Header = customer.Name,
        Tag = customer
      };
      root.Items.Add(customerItem);
      foreach (var order in db.Orders)
      {
        var orderItem = new TreeViewItem()
        {
          Header = $"{order.Description} vom {order.OrderDate:dd.MM.yyyy}",
          Tag = order,
        };
        customerItem.Items.Add(orderItem);
        foreach (var detail in db.OrderDetails)
        {
          var detailItem = new TreeViewItem()
          {
            Header = $"{detail.Amount} {detail.Product.Description} zu je {detail.Product.Price}",
            Tag = detail,
          };
          orderItem.Items.Add(detailItem);
        }
      }
    }
  }

  private void DisplayProducts(){
    lsbProducts.ItemsSource = db.Products.ToList();
  }

  private void SeedData()
  {
    Console.WriteLine("Seeding Data");
    db.Customers.AddRange([
        new Customer(){
          Name = "Firma Berger",
          Latitude = 48.3352,
          Longitude = 14.5324,
        },
        new Customer(){
          Name = "Fam. Lehner",
          Latitude = 48.5136,
          Longitude = 14.1902,
        },
      ]);
    db.Products.AddRange([
        new Product(){
          Weight = 43,
          Price = 19,
          Description = "Platten A",
        },
        new Product(){
          Weight = 46,
          Price = 22,
          Description = "Platten C",
        },
        new Product(){
          Weight = 52,
          Price = 31,
          Description = "Platten B",
        },
        new Product(){
          Weight = 2,
          Price = 10,
          Description = "Isolierung B",
        },
        new Product(){
          Weight = 2,
          Price = 11,
          Description = "Isolierung C",
        },
        new Product(){
          Weight = 1,
          Price = 8,
          Description = "Isolierung D",
        },
        new Product(){
          Weight = 3,
          Price = 12,
          Description = "Isolierung A",
        },
      ]);
    db.Orders.AddRange([
        new Order(){
          Description = "Plattenlieferung 1",
          OrderDate = DateTime.ParseExact("11.02.2021","dd.MM.yyyy",null),
          CustomerId = 1,
        },
        new Order(){
          Description = "Hausisolierung 32",
          OrderDate = DateTime.ParseExact("12.02.2021","dd.MM.yyyy",null),
          CustomerId = 2,
        },
        new Order(){
          Description = "Hausisolierung 33",
          OrderDate = DateTime.ParseExact("05.02.2021","dd.MM.yyyy",null),
          CustomerId = 2,
        },
      ]);
    db.SaveChanges();
    db.OrderDetails.AddRange([
        new OrderDetail(){
          Amount = 15,
          OrderId = 1,
          ProductId = 1,
        },
        new OrderDetail(){
          Amount = 20,
          OrderId = 1,
          ProductId = 2,
        },
        new OrderDetail(){
          Amount = 30,
          OrderId = 3,
          ProductId = 3,
        },
        new OrderDetail(){
          Amount = 60,
          OrderId = 2,
          ProductId = 4,
        },
        new OrderDetail(){
          Amount = 20,
          OrderId = 2,
          ProductId = 5,
        },
        new OrderDetail(){
          Amount = 20,
          OrderId = 2,
          ProductId = 6,
        },
      ]);
    db.Employees.AddRange([
        new Employee(){
          FirstName= "Hansi",
          LastName = "Huber",
        },
        new Employee(){
          FirstName= "Susi",
          LastName = "Maier",
        },
        new Employee(){
          FirstName= "Fritzi",
          LastName = "Müller",
        },
        new Employee(){
          FirstName= "Franzi",
          LastName = "Hehenberger",
        },
        new Employee(){
          FirstName= "Pauli",
          LastName = "Gruber",
        },
        new Employee(){
          FirstName= "Elfi",
          LastName = "Gerber",
        },
        new Employee(){
          FirstName= "Maxi",
          LastName = "Moser",
        },
      ]);
    db.SaveChanges();
    Console.WriteLine("Seeded Successfully");
    Title = db.OrderDetails.Count().ToString();
  }

  private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
  {
    var random = new Random();
    db.Products.Add(new Product
    {
      Description = txtProduct.Text,
      Price = random.Next(0, 100),
      Weight = random.Next(0, 100),
    });
    db.SaveChanges();
    DisplayProducts();
  }

  private void txtEmployees_TextChanged(object sender, TextChangedEventArgs e)
  {
    DisplayFilteredEmployees();
    DisplayShipments();
  }

  private void ButtonSelectEmployee_Click(object sender, RoutedEventArgs e)
  {
    var selectedEmployee = txtEmployees.Tag as Employee;
    if (selectedEmployee == null) return;
    if (dtpDeliveryDate.SelectedDate == null) return;
    var selectedOrder = ((trvCustomers.SelectedItem as TreeViewItem)?.Tag as Order)??null;
    if (selectedOrder == null) return;
    if (selectedOrder.Shipment != null) return;
    db.Shipments.Add(new Shipment()
    {
      PlanDate = (DateTime)dtpDeliveryDate.SelectedDate!,
      Employee = selectedEmployee,
      SequenceNr = _sequence,
    });
    db.SaveChanges();
    db.Orders.Single(x=>x.Id == selectedOrder.Id).ShipmentId = _sequence;
    db.SaveChanges();
    _sequence++;
    DisplayShipments();
  }

  private void lsbEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (lsbEmployees.SelectedItem == null) return;
    txtEmployees.Tag = lsbEmployees.SelectedItem;
    txtEmployees.Text = (lsbEmployees.SelectedItem as Employee)?.Name??"Unknown";
    DisplayShipments();
  }

  private void DisplayShipments() {
    var selectedEmployee = txtEmployees.Tag as Employee;
    if (selectedEmployee == null)
    {
      dgShipments.ItemsSource = db.Shipments
        .Where(x => dtpDeliveryDate.SelectedDate == null ? true : dtpDeliveryDate.SelectedDate == x.PlanDate)
        .Select(DgRow.FromShipment)
        .ToList();
      return;
    }
    var shipments = db.Shipments
      .Where(x => x.EmployeeId == selectedEmployee.Id)
      .Where(x => dtpDeliveryDate.SelectedDate == null ? true : dtpDeliveryDate.SelectedDate == x.PlanDate)
      .Select(DgRow.FromShipment)
      .ToList();
    dgShipments.ItemsSource = shipments;
  }

  private void dtpDeliveryDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
  {
    DisplayShipments();
  }
}
