using CommissionApp.Components.CsvReader.Extensions;
using CommissionApp.Data.Entities;

namespace CommissionApp.Components.CsvReader;
public class CsvReader : ICsvReader
{
    public List<Car> ProcessCars(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Car>();
        }
        var cars = File.ReadAllLines(filePath)
            .Skip(1)
            .Where(x => x.Length > 1)
            .ToCar()
            ;
        return cars.ToList();
    }
    public List<Customer> ProcessCustomers(string filePathCustomer)
    {
        if (!File.Exists(filePathCustomer))
        {
            return new List<Customer>();
        }
        var customers = File.ReadAllLines(filePathCustomer)
           .Skip(1)
           .Where(x => x.Length > 1)
           .ToCustomer()
           ;
        return customers.ToList();
    }
}

