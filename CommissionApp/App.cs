namespace CommissionApp;

using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using CommissionApp.Data;
using CommissionApp.Components.CsvReader;
using CommissionApp.Data.Entities;
using CommissionApp.Audit.InputToSqlAuditTxtFile;
using CommissionApp.JsonFile.ImportCsvToSqlExportJsonFile;
using System.IO;
using System;
using static System.Collections.Specialized.BitVector32;

public class App : IApp
{
    private readonly CommissionAppSQLDbContext _commissionAppSQLDbContext;
    private readonly ICsvReader _csvReader;
    private readonly IJsonFileService<Customer> _jsonCustomerService;
    private readonly IJsonFileService<Car> _jsonCarService;
    private readonly IAudit _auditRepository;
    public App(CommissionAppSQLDbContext commissionAppSQLDbContext,
               ICsvReader csvReader,
               IJsonFileService<Customer> jsonCustomerService,
               IJsonFileService<Car> jsonCarService,
               IAudit auditRepository)
    {
        _csvReader = csvReader;
        _jsonCustomerService = jsonCustomerService;
        _jsonCarService = jsonCarService;
        _auditRepository = auditRepository;
        _commissionAppSQLDbContext = commissionAppSQLDbContext;
        _commissionAppSQLDbContext.Database.EnsureCreated();
    }
    public void Run()
    {
        string action = "[System report: Error!]";
        string itemData = "[System report: Error!]";
        var auditRepository = new JsonAudit($"{action}", $"{itemData}");

        string input;
        do
        {
            TextColoring(ConsoleColor.Green, "Welcome to Aplication Commission App");
            Console.WriteLine("\n================= MENU Data Cars and Customers ================");
            Console.WriteLine("1. Add customer to SQL");
            Console.WriteLine("2. Add car to SQL");
            Console.WriteLine("3. Display all customers from SQL");
            Console.WriteLine("4. Display all cars from SQL");
            Console.WriteLine("5. Display all customers and cars from SQL");
            Console.WriteLine("6. Remove all customers from SQL");
            Console.WriteLine("7. Remove all cars from SQL");
            Console.WriteLine("8. Remove all customers and cars from SQL");
            Console.WriteLine("9. Remove customer by Id");
            Console.WriteLine("10. Remove car by Id");
            Console.WriteLine("11. Display audit file");
            Console.WriteLine("12. Import data to SQL from file Cars.csv");
            Console.WriteLine("13. Import data to SQL from file Customers.csv");
            Console.WriteLine("14. Create file Customers.json and Car.json from Customers.csv and Car.csv");
            Console.WriteLine("15. Load data from files Customers.json and Cars.json to SQL");
            Console.WriteLine("16. Order data cars from SQL by prices ");
            Console.WriteLine("17. Display cars more expensive than 500.000 SQL ");
            Console.WriteLine("18. Didplay customers who can buy cars for the given price ");
            Console.WriteLine("19. Cars that a customer can buy based on his price ");
            Console.WriteLine("20. Export cars grouped by customers from SQL to file CarsByCustomers.xml ");
            Console.WriteLine("21. Create file Cars.xmL from files Cars.csv");
            Console.WriteLine(" Press q to exit program: ");
            input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1":
                    {
                        AddCustomerToSQL();
                    }
                    break;
                case "2":
                    {
                        AddCarToSQL();
                    }
                    break;
                case "3":
                    {
                        ReadCustomersFromDbSQL();
                    }
                    break;
                case "4":
                    {
                        ReadCarsFromDbSQL();
                    }
                    break;
                case "5":
                    {
                        Console.WriteLine("Cars List:");
                        ReadCustomersFromDbSQL();
                        Console.WriteLine("Customers List:");
                        ReadCarsFromDbSQL();
                    }
                    break;
                case "6":
                    {
                        TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers - -");
                        DeleteAllCustomers();
                    }
                    break;
                case "7":
                    {
                        TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an cars - -");
                        DeleteAllCars();
                    }
                    break;
                case "8":
                    {
                        TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers and cars - -");
                        DeleteAllCustomers();
                        DeleteAllCars();
                    }
                    break;
                case "9":
                    {
                        TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customer - -");
                        RemoveCustomerById();
                    }
                    break;
                case "10":
                    {
                        TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an car - -");
                        RemoveCarById();
                    }
                    break;
                case "11":
                    {
                        WriteAllFromAuditFileToConsole();
                    }
                    break;
                case "12":
                    {
                        Console.WriteLine("Imort Data Car From file.csv to Sql by method: InsertDataToSQLFromCsv:");
                        Console.WriteLine("Creating Audit File: Car.TXT:");
                        InsertDataToSQLFromCsv();
                    }
                    break;
                case "13":
                    {
                        Console.WriteLine("Import Data Customer from File Csv to Sql 13:");
                        Console.WriteLine("Creating Audit File: Customer.TXT:");
                        InsertDataCustomersToSQLFromCsv();
                    }
                    break;
                case "14":
                    {
                        Console.WriteLine("Creating file.json from Customer.CSV an Car.CSV 14:");
                        ExportToJsonFileSqlRepo(_commissionAppSQLDbContext);
                    }
                    break;
                case "15":
                    {
                        Console.WriteLine("Load data from files json 15:");
                        LoadDataFromJsonFiles();
                    }
                    break;
                case "16":
                    {
                        Console.WriteLine("Order data cars by prices SQL ");
                        try
                        {
                            var cars = _commissionAppSQLDbContext
                            .Cars
                            .OrderByDescending(car => car.CarPrice)
                            .ToList();

                            if (cars.Any())
                            {
                                Console.WriteLine("List of Cars from the Database:");
                                foreach (var car in cars)
                                {
                                    Console.WriteLine($"\tID: {car.Id} | Brand: {car.CarBrand} | Model: {car.CarModel} | Price: {car.CarPrice:C}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No cars found in the database.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while reading cars from the database: {ex.Message}");
                        }
                    }
                    break;
                case "17":
                    {
                        Console.WriteLine("Cars more expensive than 500.000 SQL ");
                        try
                        {
                            var cars = _commissionAppSQLDbContext
                          .Cars
                                        .Where(car => car.CarPrice > 500000)
                                        .ToList();
                            if (cars.Any())
                            {
                                Console.WriteLine("List of Cars from the Database:");
                                foreach (var car in cars)
                                {
                                    Console.WriteLine($"\tID: {car.Id} | Brand: {car.CarBrand} | Model: {car.CarModel} | Price: {car.CarPrice:C}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No cars found in the database.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while reading cars from the database: {ex.Message}");
                        }
                    }
                    break;
                case "18":
                    {
                        Console.WriteLine("18. Customers and the cars they can buy at the given price ");
                        GroupCustomersWithCarsByPrice();

                    }
                    break;
                case "19":
                    {
                        Console.WriteLine("19. Cars that a customer can buy based on his Customer Price ");
                        DisplayAffordableCarsGroupedByCustomers();

                    }
                    break;
                case "20":
                    {
                        ExportCarsGroupedByCustomersToXml();
                    }
                    break;
                case "21":
                    {
                        ExportCarsToXml();
                        CreateXmL();
                    }
                    break;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    break;
            }
        } while (input != "q");
    }

    private bool AddCustomerToSQL()
    {
        string action = "Customer Added!";
        string itemData = "Customer added to SQL";
        var auditRepository = new JsonAudit($"{action}", $"{itemData}");
        Console.WriteLine("Adding Customer :");
        Console.Write("Enter first name: ");
        var firstname = Console.ReadLine();
        Console.Write("Enter last name: ");
        var lastname = Console.ReadLine();
        Console.Write("Is Premium Client ( true or false): ");
        var email = Console.ReadLine();
        Console.Write("Enter Price: ");
        var price = Console.ReadLine();
        if (firstname != null && lastname != null && email != null && price != null)
        {
            if (firstname == "")
            {
                firstname = "unknown";
            }
            if (lastname == "")
            {
                lastname = "unknown";
            }
            if (email != "true" && email != "false")
            {
                email = "false";
            }
            while (float.TryParse(price, out float stringToFloat) == false)
            {
                Console.Write("the value is incorrect!  ");
                price = Console.ReadLine();
            }

            _commissionAppSQLDbContext.Customers.Add(new Customer { FirstName = firstname, LastName = lastname, Email = bool.Parse(email), Price = decimal.Parse(price) });
            _commissionAppSQLDbContext.SaveChanges();
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RemoveCustomerById()
    {
        string action = "Customer removed ";
        string itemData = "Customer added to SQL";
        var auditRepository = new JsonAudit($"{action}", $"{itemData}");
        Console.WriteLine("Removing Customer:");
        Console.Write("Enter Customer ID to remove: ");
        var idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int customerId))
        {
            var customer = _commissionAppSQLDbContext.Customers.Find(customerId);

            if (customer != null)
            {
                _commissionAppSQLDbContext.Customers.Remove(customer);
                _commissionAppSQLDbContext.SaveChanges();
                Console.WriteLine($"Customer with ID {customerId} has been removed.");
                auditRepository.AddEntryToFile();
                auditRepository.SaveAuditFile();
                return true;
            }
            else
            {
                Console.WriteLine($"Customer with ID {customerId} does not exist.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid ID entered. Please enter a numeric value.");
            return false;
        }
    }

    private bool AddCarToSQL()
    {
        string action = "Car Added!";
        string itemData = "Car added to SQL";
        var auditRepository = new JsonAudit($"{action}", $"{itemData}");
        Console.WriteLine("\nAdding Car :");
        Console.Write("Enter car's brand : ");
        var carbrand = Console.ReadLine();
        Console.Write("Enter car's model : ");
        var carmodel = Console.ReadLine();
        Console.Write("Enter car's price : ");
        var carprice = Console.ReadLine();
        decimal.TryParse(carprice, out decimal stringToFloat);
        _commissionAppSQLDbContext.Cars.Add(new Car { CarBrand = carbrand, CarModel = carmodel, CarPrice = decimal.Parse(carprice) });
        _commissionAppSQLDbContext.SaveChanges();
        auditRepository.AddEntryToFile();
        auditRepository.SaveAuditFile();
        return true;
    }

    private bool RemoveCarById()
    {
        string action = "Car removed!";
        string itemData = "Car removed from SQL";
        var auditRepository = new JsonAudit($"{action}", $"{itemData}");
        Console.WriteLine("Removing Customer:");
        Console.Write("Enter Customer ID to remove: ");
        var idInput = Console.ReadLine();

        if (int.TryParse(idInput, out int carId))
        {
            var car = _commissionAppSQLDbContext.Customers.Find(carId);

            if (car != null)
            {
                _commissionAppSQLDbContext.Customers.Remove(car);
                _commissionAppSQLDbContext.SaveChanges();
                Console.WriteLine($"Customer with ID {carId} has been removed.");
                auditRepository.AddEntryToFile();
                auditRepository.SaveAuditFile();
                return true;
            }
            else
            {
                Console.WriteLine($"Customer with ID {carId} does not exist.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid ID entered. Please enter a numeric value.");
            return false;
        }
    }

    private void DeleteAllCustomers()
    {
        var allCustomers = _commissionAppSQLDbContext.Customers.ToList();
        if (allCustomers.Any())
        {
            _commissionAppSQLDbContext.Customers.RemoveRange(allCustomers);
            _commissionAppSQLDbContext.SaveChanges();
            Console.WriteLine("All customers have been deleted successfully.");
        }
        else
        {
            Console.WriteLine("No customers found in the database.");
        }
    }

    private void DeleteAllCars()
    {
        var allCars = _commissionAppSQLDbContext.Cars.ToList();
        if (allCars.Any())
        {
            _commissionAppSQLDbContext.Cars.RemoveRange(allCars);
            _commissionAppSQLDbContext.SaveChanges();
            Console.WriteLine("All cars have been deleted successfully.");
        }
        else
        {
            Console.WriteLine("No cars found in the database.");
        }
    }

    private void ReadCarsFromDbSQL()
    {
        try
        {
            var cars = _commissionAppSQLDbContext
                .Cars.ToList();
            if (cars.Any())
            {
                Console.WriteLine("List of Cars from the Database:");
                foreach (var car in cars)
                {
                    Console.WriteLine($"\tID: {car.Id} | Brand: {car.CarBrand} | Model: {car.CarModel} | Price: {car.CarPrice:C}");
                }
            }
            else
            {
                Console.WriteLine("No cars found in the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading cars from the database: {ex.Message}");
        }
    }

    private void ReadCustomersFromDbSQL()
    {
        try
        {
            var customers = _commissionAppSQLDbContext
                .Customers
                .ToList();

            if (customers.Any())
            {
                Console.WriteLine("List of Customers from the Database:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"\tID: {customer.Id} | First Name: {customer.FirstName} | Last Name: {customer.LastName} | Premium Client: {customer.Email} | Price: {customer.Price:C}");
                }
            }
            else
            {
                Console.WriteLine("No customers found in the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading customers from the database: {ex.Message}");
        }
    }

    private void WriteAllFromAuditFileToConsole()
    {
        string action = "Write to Console";
        string itemData = "Write";
        var auditRepository = new JsonAudit($"{action}", $"{itemData}");
        var items = auditRepository.ReadAuditFile();
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }
    private static void TextColoring(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();

    }

    private void InsertDataToSQLFromCsv()
    {
        try
        {
            var cars = _csvReader.ProcessCars("Resources\\Files\\Cars.csv");

            if (cars == null || !cars.Any())
            {
                Console.WriteLine("No data found in the CSV file. Please check the file and try again.");
                return;
            }

            foreach (var car in cars)
            {
                try
                {
                    _commissionAppSQLDbContext.Cars.Add(new Car()
                    {
                        CarBrand = car.CarBrand,
                        CarModel = car.CarModel,
                        CarPrice = car.CarPrice,
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to add car {car.CarBrand} {car.CarModel}: {ex.Message}");
                }
            }
            try
            {
                _commissionAppSQLDbContext.SaveChanges();
                Console.WriteLine("Data successfully inserted into the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving data to the database: {ex.Message}");
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"The CSV file was not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private void InsertDataCustomersToSQLFromCsv()
    {
        try
        {
            var customers = _csvReader.ProcessCustomers("Resources\\Files\\Customers.csv");

            if (customers == null || !customers.Any())
            {
                Console.WriteLine("No data found in the CSV file. Please check the file and try again.");
                return;
            }

            foreach (var customer in customers)
            {
                try
                {
                    _commissionAppSQLDbContext.Customers.Add(new Customer()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Price = customer.Price,
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to add customer {customer.FirstName} {customer.LastName}: {ex.Message}");
                }
            }
            try
            {
                _commissionAppSQLDbContext.SaveChanges();
                Console.WriteLine("Data successfully inserted into the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving data to the database: {ex.Message}");
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"The CSV file was not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    private void GroupCustomersWithCarsByPrice()
    {
        var groupedData = _commissionAppSQLDbContext.Customers
            .Join(
                _commissionAppSQLDbContext.Cars,
                customer => customer.Price,
                car => car.CarPrice,
                (customer, car) => new
                {
                    Customer = customer,
                    Car = car
                }
            )
            .GroupBy(x => x.Customer.Price)
            .ToList();

        foreach (var group in groupedData)
        {
            Console.WriteLine($"Group for Price: {group.Key}");

            foreach (var entry in group)
            {
                Console.WriteLine($"\tCustomer: {entry.Customer.FirstName} {entry.Customer.LastName}, Price: {entry.Customer.Price}");
                Console.WriteLine($"\tCar: {entry.Car.CarBrand} {entry.Car.CarModel}, Price: {entry.Car.CarPrice}");
            }
        }
    }
    private void DisplayAffordableCarsGroupedByCustomers()
    {
        var groupedData = _commissionAppSQLDbContext.Customers
            .Select(customer => new
            {
                Customer = customer,
                AffordableCars = _commissionAppSQLDbContext.Cars
                    .Where(car => car.CarPrice <= customer.Price)
                    .OrderBy(car => car.CarPrice)
                    .ToList()
            })
            .Where(x => x.AffordableCars.Any())
            .OrderByDescending(x => x.Customer.Price)
            .ToList();

        foreach (var entry in groupedData)
        {
            Console.WriteLine($"Customer: {entry.Customer.FirstName} {entry.Customer.LastName}, Max Price: {entry.Customer.Price}");

            foreach (var car in entry.AffordableCars)
            {
                Console.WriteLine($"\tCar: {car.CarBrand} {car.CarModel}, Price: {car.CarPrice}");
            }
        }
    }
    private void CreateXmL()
    {
        var records = _csvReader.ProcessCars("Resources\\Files\\Cars.csv");
        var document = new XDocument();
        var cars = new XElement("Cars", records
            .Select(x =>
            new XElement("Car",
                new XAttribute("CarBrand", x.CarBrand),
                 new XAttribute("CarName", x.CarModel),
                  new XAttribute("CarPrice", x.CarPrice))));
        document.Add(cars);
        _auditRepository.AddEntryToFile();
        _auditRepository.SaveAuditFile();
        document.Save("Cars.xml");
    }

    private void ExportCarsToXml()
    {
        var cars = _commissionAppSQLDbContext.Cars
            .Select(car => new
            {
                car.CarBrand,
                car.CarModel,
                car.CarPrice
            })
            .ToList();

        var document = new XDocument(
            new XElement("Cars",
                cars.Select(car =>
                    new XElement("Car",
                        new XAttribute("CarBrand", car.CarBrand),
                        new XAttribute("CarName", car.CarModel),
                        new XAttribute("CarPrice", car.CarPrice)
                    )
                )
            )
        );
        _auditRepository.AddEntryToFile();
        _auditRepository.SaveAuditFile();
        document.Save("Cars.xml");

        Console.WriteLine("Data has been successfully exported to Cars.xml.");
    }


    private void ExportCarsGroupedByCustomersToXml()
    {
        var groupedData = _commissionAppSQLDbContext.Customers
            .Select(customer => new
            {
                Customer = customer,
                AffordableCars = _commissionAppSQLDbContext.Cars
                    .Where(car => car.CarPrice <= customer.Price)
                    .OrderBy(car => car.CarPrice)
                    .ToList()
            })
            .Where(x => x.AffordableCars.Any())
            .OrderByDescending(x => x.Customer.Price)
            .ToList();

        var document = new XDocument(
            new XElement("Customers",
                groupedData.Select(entry =>
                    new XElement("Customer",
                        new XAttribute("FirstName", entry.Customer.FirstName),
                        new XAttribute("LastName", entry.Customer.LastName),
                        new XAttribute("MaxPrice", entry.Customer.Price),
                        new XElement("AffordableCars",
                            entry.AffordableCars.Select(car =>
                                new XElement("Car",
                                    new XAttribute("CarBrand", car.CarBrand),
                                    new XAttribute("CarModel", car.CarModel),
                                    new XAttribute("CarPrice", car.CarPrice)
                                )
                            )
                        )
                    )
                )
            )
        );

        document.Save("CarsByCustomers.xml");
        Console.WriteLine("Data has been successfully exported to CarsByCustomers.xml.");
    }

    private void LoadDataFromJsonFiles()
    {
        var customerFromFile = _jsonCustomerService.LoadFromFile();
        var carFromFile = _jsonCarService.LoadFromFile();

        if (customerFromFile.Any())
        {
            foreach (var customer in customerFromFile)
            {
                Console.WriteLine($"ID: {customer.Id}, FirstName: {customer.FirstName}, LastName: {customer.LastName}, Premium: {customer.Email}, Price: {customer.Price:C}");
            }
        }
        if (carFromFile.Any())
        {
            foreach (var car in carFromFile)
            {
                Console.WriteLine($"ID: {car.Id}, Brand: {car.CarBrand}, Model: {car.CarModel}, Price: {car.CarPrice:C}");
            }
        }
        ExportToJsonFileSqlRepo(_commissionAppSQLDbContext);
    }
    private void ExportToJsonFileSqlRepo(CommissionAppSQLDbContext commissionAppSQLDbContext)
    {
        string filePath = "Resources\\Files\\Customers.csv";
        var customRecords = _csvReader.ProcessCustomers("Resources\\Files\\Customers.csv");
        List<Customer> customers = _csvReader.ProcessCustomers(filePath);
        foreach (var customer in customRecords)
        {
            _auditRepository.AddEntryToFile();
            _auditRepository.SaveAuditFile();
            _jsonCustomerService.SaveToFile(_commissionAppSQLDbContext.Customers);
        }
        foreach (var customer in customRecords)
        {
            Console.WriteLine($"Wypisanie lini{customer}");
        }
        string file = "Resources\\Files\\Cars.csv";
        var records = _csvReader.ProcessCars("Resources\\Files\\Cars.csv");

        List<Car> cars = _csvReader.ProcessCars(file);
        foreach (var car in records)
        {
            _auditRepository.AddEntryToFile();
            _auditRepository.SaveAuditFile();
            _jsonCarService.SaveToFile(_commissionAppSQLDbContext.Cars);
        }
        foreach (var car in records)
        {
            Console.WriteLine($"{car}");
        }
    }
}
