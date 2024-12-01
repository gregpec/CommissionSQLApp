﻿using CommissionApp.Services;
using CommissionApp.Audit.InputToSqlAuditTxtFile;
using CommissionApp.Data.Entities;
using CommissionApp.Data.Repositories;

namespace CommissionApp.UserCommunication
{
     public class UserCommunication:IUserCommunication
    {
        private readonly IDbContextService _dbContextService;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Car> _carsRepository;
      
        public UserCommunication(
             IDbContextService dbContextService
           ,IRepository<Customer> customerRepository,
              IRepository<Car> carRepository
      
            )
        {
            _dbContextService = dbContextService;
           _customersRepository = customerRepository;
          _carsRepository = carRepository;
          
        }
        void TextColoring(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public void UseUserCommunication()
        {
            string action = "[System report: Error!]";
            string itemData = "[System report: Error!]";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");

            void CustomerRepositoryOnItemAdded(object? sender, Customer e)
            {
                TextColoring(ConsoleColor.Red, $"Event: Customer {e.FirstName} added from repository => {sender?.GetType().Name}!");             
                Console.ForegroundColor = ConsoleColor.Green;
                AddAuditInfo(e, "CUSTOMER ADDED");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Customer\n{e}\nadded successfully.\n");
                Console.ResetColor();
            }

            void CarRepositoryOnItemAdded(object? sender, Car e)
            {
                TextColoring(ConsoleColor.Red, $"Event: Car {e.CarBrand} added from repository => {sender?.GetType().Name}!");
                Console.ForegroundColor = ConsoleColor.Green;
                AddAuditInfo(e, "CAR ADDED");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Car\n{e}\nadded successfully.\n");
                Console.ResetColor();
            }
            _carsRepository.ItemAdded += CarRepositoryOnItemAdded;
            _customersRepository.ItemAdded += CustomerRepositoryOnItemAdded;

            void AddAuditInfo<T>(T e, string info) where T : class, IEntity
            {
                using (var writer = File.AppendText((IRepository<IEntity>.auditFileName)))
                {
                    writer.WriteLine($"[{DateTime.UtcNow}]\t{info} :\n    [{e}]");
                }
            }

            string input;
            do
            {
                _dbContextService.TextColoring(ConsoleColor.Green, "Welcome to Aplication Commission App");
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
                            _dbContextService.AddCustomerToSQL(_customersRepository);                       
                        }
                        break;
                    case "2":
                        {                       
                            _dbContextService.AddCarToSQL(_carsRepository);                          
                        }
                        break;
                    case "3":
                        {
                            _dbContextService.ReadCustomersFromDbSQL();
                        }
                        break;
                    case "4":
                        {
                            _dbContextService.ReadCarsFromDbSQL();
                        }
                        break;
                    case "5":
                        {
                            Console.WriteLine("Cars List:");
                            _dbContextService.ReadCustomersFromDbSQL();
                            Console.WriteLine("Customers List:");
                            _dbContextService.ReadCarsFromDbSQL();
                        }
                        break;
                    case "6":
                        {
                            _dbContextService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers - -");
                            _dbContextService.DeleteAllCustomers();
                        }
                        break;
                    case "7":
                        {
                            _dbContextService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an cars - -");
                            _dbContextService.DeleteAllCars();
                        }
                        break;
                    case "8":
                        {
                            _dbContextService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers and cars - -");
                            _dbContextService.DeleteAllCustomers();
                            _dbContextService.DeleteAllCars();
                        }
                        break;
                    case "9":
                        {
                            _dbContextService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customer - -");
                            _dbContextService.RemoveCustomerById();
                        }
                        break;
                    case "10":
                        {
                            _dbContextService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an car - -");
                            _dbContextService.RemoveCarById();
                        }
                        break;
                    case "11":
                        {
                            _dbContextService.WriteAllFromAuditFileToConsole();
                        }
                        break;
                    case "12":
                        {
                            Console.WriteLine("Imort Data Car From file.csv to Sql by method: InsertDataToSQLFromCsv:");
                            Console.WriteLine("Creating Audit File: Car.TXT:");
                            _dbContextService.InsertDataToSQLFromCsv();
                        }
                        break;
                    case "13":
                        {
                            Console.WriteLine("Import Data Customer from File Csv to Sql 13:");
                            Console.WriteLine("Creating Audit File: Customer.TXT:");
                            _dbContextService.InsertDataCustomersToSQLFromCsv();
                        }
                        break;
                    case "14":
                        {
                            Console.WriteLine("Creating file.json from Customer.CSV an Car.CSV 14:");
                            _dbContextService.ExportToJsonFileSqlRepo();
                        }
                        break;
                    case "15":
                        {
                            Console.WriteLine("Load data from files json:");
                            _dbContextService.LoadDataFromJsonFiles();
                        }
                        break;
                    case "16":
                        {
                            _dbContextService.OrderCarsByPrices();
                        }
                        break;
                    case "17":
                        {
                            Console.WriteLine("Cars more expensive than 500.000 SQL ");
                            _dbContextService.CarsMoreExpensiveThan();
                        }
                        break;
                    case "18":
                        {
                            Console.WriteLine("18. Customers and the cars they can buy at the given price ");
                            _dbContextService.GroupCustomersWithCarsByPrice();

                        }
                        break;
                    case "19":
                        {
                            Console.WriteLine("19. Cars that a customer can buy based on his Customer Price ");
                            _dbContextService.DisplayAffordableCarsGroupedByCustomers();

                        }
                        break;
                    case "20":
                        {
                            _dbContextService.ExportCarsGroupedByCustomersToXml();
                        }
                        break;
                    case "21":
                        {
                            _dbContextService.ExportCarsToXml();
                            _dbContextService.CreateXmL();
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            } while (input != "q");
        }
    }
}
