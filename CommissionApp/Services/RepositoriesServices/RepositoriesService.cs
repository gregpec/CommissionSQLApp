using CommissionApp.Audit.AuditJsonFile;
using CommissionApp.Data.Entities;
using System.Xml.Linq;
using CommissionApp.Components.CsvReader;
using CommissionApp.Data.Repositories;

namespace CommissionApp.Services.RepositoriesServices
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ICsvReader _csvReader;
        private readonly IAudit _auditRepository;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Car> _carsRepository;

        public RepositoriesService
            (
               ICsvReader csvReader,
               IAudit auditRepository,
               IRepository<Customer> customerRepository,
               IRepository<Car> carRepository
            )
        {
            _csvReader = csvReader;
            _auditRepository = auditRepository;
            _customersRepository = customerRepository;
            _carsRepository = carRepository;
        }

        public bool AddCustomerToSQL(IRepository<Customer> customerRepository)
        {
            string action = "Customer Added!";
            string itemData = "Customer Added to SQL";
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

                _customersRepository.Add(new Customer()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Email = bool.Parse(email),
                    Price = decimal.Parse(price)
                });
                _customersRepository.Save();
                _auditRepository.AddEntryToFile();
                _auditRepository.SaveAuditFile();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveCustomerById(IRepository<Customer> customerRepository)
        {
            string action = "Customer Removed!";
            string itemData = "Customer removed from SQL";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            bool isIdCorrect = true;
            do
            {
                Console.WriteLine($"\nEnter the ID of the person you want to remove from the database:\n(Press 'q' button + 'ENTER' button to quit and back to main menu)");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }
                try
                {
                    isIdCorrect = int.TryParse(input, out int id);
                    _customersRepository.Remove(_customersRepository.GetById(id));
                    _customersRepository.Save();

                }
                catch (Exception exception)
                {
                    TextColoring(ConsoleColor.DarkRed, $"\nWarning! Exception catched:\n{exception.Message}\n");
                    TextColoring(ConsoleColor.DarkRed, "This ID is not existing! Try again!\n\t(Tip: View the list from the main menu to check the ID of the person you want to remove from the database)\n");
                }
                finally
                {
                    //_commissionAppSQLDbContext.SaveChanges();
                    _customersRepository.Save();
                    auditRepository.AddEntryToFile();
                    auditRepository.SaveAuditFile();
                }
            } while (false);
        }

        public bool AddCarToSQL(IRepository<Car> carRepository)
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
            _carsRepository.Add(new Car { CarBrand = carbrand, CarModel = carmodel, CarPrice = decimal.Parse(carprice) });
            _carsRepository.Save();
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
            return true;
        }
        public void WriteAllCustomersToConsole(IRepository<Customer> customerRepository)
        {
            var items = _customersRepository.GetAll();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
        public void WriteAllCarsToConsole(IRepository<Car> carRepository)
        {
            var items = _carsRepository.GetAll();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
        public void RemoveCarById(IRepository<Car> carRepository)
        {
            string action = "Car Removed!";
            string itemData = "Car removed from SQL by Car Repository";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            bool isIdCorrect = true;
            do
            {
                Console.WriteLine($"\nEnter the ID of the person you want to remove from the database:\n(Press 'q' button + 'ENTER' button to quit and back to main menu)");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }
                try
                {
                    isIdCorrect = int.TryParse(input, out int id);
                    _carsRepository.Remove(_carsRepository.GetById(id));
                }
                catch (Exception exception)
                {
                    TextColoring(ConsoleColor.DarkRed, $"\nWarning! Exception catched:\n{exception.Message}\n");
                    TextColoring(ConsoleColor.DarkRed, "This ID is not existing! Try again!\n\t(Tip: View the list from the main menu to check the ID of the person you want to remove from the database)\n");
                }
                finally
                {
                    _carsRepository.Save();
                    auditRepository.AddEntryToFile();
                    auditRepository.SaveAuditFile();
                }
            } while (false);
        }

        public void DeleteAllCustomers(IRepository<Customer> customerRepository)
        {
            string action = "Delete all Customers!";
            string itemData = "Deleting all Customers!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            var allCustomers = _customersRepository.GetAll();
            if (allCustomers.Any())
            {
                _customersRepository.RemoveAll();
                _customersRepository.Save();
                auditRepository.AddEntryToFile();
                auditRepository.SaveAuditFile();
                Console.WriteLine("All customers have been deleted successfully.");
            }
            else
            {
                Console.WriteLine("No customers found in the database.");
            }
        }
        public void DeleteAllCars(IRepository<Car> carRepository)
        {
            string action = "Deleting all Cars";
            string itemData = "Deleting all Cars!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            var allCars = _carsRepository.GetAll();
            if (allCars.Any())
            {
                _carsRepository.RemoveAll();
                _carsRepository.Save();
                Console.WriteLine("All cars have been deleted successfully.");
            }
            else
            {
                Console.WriteLine("No cars found in the database.");
            }
        }
       
        public void TextColoring(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void InsertDataCarsToSQLFromCsv(IRepository<Car> carRepository)
        {
            string action = "Insert Data Cars to Repositorie and Sql from Csv";
            string itemData = "insenrt data cars to Sql";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
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
                        _carsRepository.Add(new Car()
                        {
                            Id = car.Id,
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
                    _carsRepository.Save();
                    auditRepository.AddEntryToFile();
                    auditRepository.SaveAuditFile();
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
        public void InsertDataCustomersToSQLFromCsv(IRepository<Customer> customerRepository)
        {
            string action = "Insering customers drom csv to sql";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
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
                        _customersRepository.Add(new Customer()
                        {
                            Id = customer.Id,
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

                    _customersRepository.Save();
                    auditRepository.AddEntryToFile();
                    auditRepository.SaveAuditFile();
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
        public void GroupCustomersWithCarsByPrice(IRepository<Customer> customerRepository, IRepository<Car> carRepository)
        {
            string action = "GroupCustomersWithCarsByPrice";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            var groupedData = _customersRepository.GetAll()
                .Join(
                   _carsRepository.GetAll(),
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
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
        }
        public void DisplayAffordableCarsGroupedByCustomers(IRepository<Customer> customerRepository, IRepository<Car> carRepository)
        {
            string action = "DisplayAffordableCarsGroupedByCustomers";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            var groupedData = _customersRepository.GetAll()
                .Select(customer => new
                {
                    Customer = customer,
                    AffordableCars = _carsRepository.GetAll()
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
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
        }

        public void ExportCarsToXml(IRepository<Car> carRepository)
        {
            string action = "ExportCarsToXml";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            var cars = _carsRepository.GetAll()
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
            document.Save("Cars.xml");
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
            Console.WriteLine("Data has been successfully exported to Cars.xml.");
        }
        public void ExportCarsGroupedByCustomersToXml(IRepository<Customer> customerRepository, IRepository<Car> carRepository)
        {
            string action = "ExportCarsGroupedByCustomersToXml";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            var groupedData = _customersRepository.GetAll()
                .Select(customer => new
                {
                    Customer = customer,
                    AffordableCars = _carsRepository.GetAll()
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
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
            document.Save("CarsByCustomers.xml");
            Console.WriteLine("Data has been successfully exported to CarsByCustomers.xml.");
        }
       
       
        public void OrderCarsByPrices(IRepository<Car> carRepository)
        {
            Console.WriteLine("Order data cars by prices SQL ");
            try
            {
                var cars = _carsRepository.GetAll()
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
        public void CarsMoreExpensiveThan(IRepository<Car> carRepository)
        {
            try
            {
                var cars = _carsRepository.GetAll()
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


