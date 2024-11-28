using CommissionApp.Audit.InputToSqlAuditTxtFile;
using CommissionApp.Data.Entities;
using CommissionApp.Data;
using System.Xml.Linq;
using CommissionApp.Components.CsvReader;
using CommissionApp.JsonFile.ImportCsvToSqlExportJsonFile;

namespace CommissionApp.Services
{
    public class DbContextService : IDbContextService
    {
        private readonly CommissionAppSQLDbContext _commissionAppSQLDbContext;
        private readonly ICsvReader _csvReader;
        private readonly IJsonFileService<Customer> _jsonCustomerService;
        private readonly IJsonFileService<Car> _jsonCarService;
        private readonly IAudit _auditRepository;

        public DbContextService
            (  CommissionAppSQLDbContext commissionAppSQLDbContext,
               ICsvReader csvReader,
               IJsonFileService<Customer> jsonCustomerService,
               IJsonFileService<Car> jsonCarService,
               IAudit auditRepository
            )
        {
            _csvReader = csvReader;
            _jsonCustomerService = jsonCustomerService;
            _jsonCarService = jsonCarService;
            _auditRepository = auditRepository;
            _commissionAppSQLDbContext = commissionAppSQLDbContext;
            _commissionAppSQLDbContext.Database.EnsureCreated();
        }
        public bool AddCustomerToSQL()
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
        public bool RemoveCustomerById()
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
        public bool AddCarToSQL()
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
        public bool RemoveCarById()
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
        public void DeleteAllCustomers()
        {
            string action = "Delete all Customers!";
            string itemData = "Deleting all Customers!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
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
        public void DeleteAllCars()
        {
            string action = "Deleting all Cars";
            string itemData = "Deleting all Cars!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
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
        public void ReadCarsFromDbSQL()
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
        public void ReadCustomersFromDbSQL()
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
        public void WriteAllFromAuditFileToConsole()
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
        public void TextColoring(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();

        }
        public void InsertDataToSQLFromCsv()
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
                    string action = "Insering data drom csv to sql";
                    string itemData = "!";
                    var auditRepository = new JsonAudit($"{action}", $"{itemData}");
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
        public void InsertDataCustomersToSQLFromCsv()
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
                    string action = "Insering customers drom csv to sql";
                    string itemData = "!";
                    var auditRepository = new JsonAudit($"{action}", $"{itemData}");
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
        public void GroupCustomersWithCarsByPrice()
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
        public void DisplayAffordableCarsGroupedByCustomers()
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
        public void CreateXmL()
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
        public void ExportCarsToXml()
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
        public void ExportCarsGroupedByCustomersToXml()
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
        public void LoadDataFromJsonFiles()
        {
            string action = "Load data from jsom to console";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            _auditRepository.AddEntryToFile();
            _auditRepository.SaveAuditFile();
            var customerFromFile = _jsonCustomerService.LoadFromFile();
            var carFromFile = _jsonCarService.LoadFromFile();

            if (customerFromFile.Any())
            {
                foreach (var customer in customerFromFile)
                {
                    Console.WriteLine($"ID: {customer.Id}, FirstName: {customer.FirstName}, LastName: {customer.LastName}, Premium: {customer.Email}, Price: {customer.Price:C}");
                }
            }
            else
            {
                Console.WriteLine("No files json");
            }
            if (carFromFile.Any())
            {
                foreach (var car in carFromFile)
                {
                    Console.WriteLine($"ID: {car.Id}, Brand: {car.CarBrand}, Model: {car.CarModel}, Price: {car.CarPrice:C}");
                }
            }
            else
            {
                Console.WriteLine("No files json");
            }
        }
        public void ExportToJsonFileSqlRepo()
        {
            string filePath = "Resources\\Files\\Customers.csv";
            var customRecords = _csvReader.ProcessCustomers("Resources\\Files\\Customers.csv");
            List<Customer> customers = _csvReader.ProcessCustomers(filePath);

            foreach (var customer in customRecords)
            {
                string action = "Load data customers from csv sql";
                string itemData = "!";
                var auditRepository = new JsonAudit($"{action}", $"{itemData}");

                Console.WriteLine($"Wypisanie lini{customer}");
                _jsonCustomerService.SaveToFile(_commissionAppSQLDbContext.Customers);
                auditRepository.AddEntryToFile();
                auditRepository.SaveAuditFile();
            }
            string file = "Resources\\Files\\Cars.csv";
            var records = _csvReader.ProcessCars("Resources\\Files\\Cars.csv");

            List<Car> cars = _csvReader.ProcessCars(file);
            foreach (var car in records)
            {
                string action = "Load data cars from csv sql";
                string itemData = "!";
                var auditRepository = new JsonAudit($"{action}", $"{itemData}");
                auditRepository.AddEntryToFile();
                auditRepository.SaveAuditFile();
                _jsonCarService.SaveToFile(_commissionAppSQLDbContext.Cars);
            }
            foreach (var car in records)
            {
                Console.WriteLine($"{car}");
            }
        }
        public void OrderCarsByPrices()
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
        public void CarsMoreExpensiveThan()
        {
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
    }
}


