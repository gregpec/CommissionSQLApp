using System.IO;
namespace CommissionApp.Helpers;

public static class
FilePaths
{
    public static readonly string 
    Resources = Path.Combine(AppContext.BaseDirectory,"Resources","Files"); 
    public static readonly string
    CarsCsv = Path.Combine(Resources,"Cars.csv");
    public static readonly string
    CustomersCsv = Path.Combine(Resources,"Customers.csv");
    public static readonly string
    AuditTxt = Path.Combine(Resources,"Audit.txt");
    public static readonly string
    AuditFileJson = Path.Combine(Resources,"AuditFile.json");
    public static readonly string
    CarsXml = Path.Combine(Resources,"Cars.xml");
    public static readonly string
    CarsByCustomersXml = Path.Combine(Resources,"CarsByCustomers.xml");
    public static readonly string
    CarsJson = Path.Combine(Resources, "Cars.json");
    public static readonly string
    CustomersJson = Path.Combine(Resources, "Customers.json");
}
