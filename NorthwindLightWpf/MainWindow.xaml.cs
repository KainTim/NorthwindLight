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
}
