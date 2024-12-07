using CommissionApp.Data;
using CommissionApp.Data.Entities;
using CommissionApp.Data.Repositories;
using CommissionApp.Services.RepositoriesServices;
using CommissionApp.Services.FilesServices.JsonFile;
using CommissionApp.Audit.AuditJsonFile;

namespace CommissionApp.UserCommunication
{
    public class UserCommunication:IUserCommunication
    {
        private readonly IRepositoriesService _repositoriesService;
        private readonly IEventHandlerService _eventHandlerService;
        private readonly IJsonServices _jsonServices;
        private readonly IAudit _auditRepository;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Car> _carsRepository;
      
        public UserCommunication(
                                IRepositoriesService repositoriesService,
                                IRepository<Customer> customerRepository,
                                IRepository<Car> carRepository,
                                IEventHandlerService eventHandlerService,
                                IJsonServices jsonServices,
                                IAudit auditRepository
                                )
        {
            _repositoriesService = repositoriesService;
            _customersRepository = customerRepository;
            _carsRepository = carRepository;
            _eventHandlerService = eventHandlerService;
            _jsonServices = jsonServices;
            _auditRepository = auditRepository;
         }

        public void UseUserCommunication()
        {
            _eventHandlerService.Events();
           
            string input;
            do
            {
                _repositoriesService.TextColoring(ConsoleColor.Green, "Welcome to Aplication Commission App");
                Console.WriteLine("\n================= MENU Data Cars and Customers ================");
                Console.WriteLine("1. Add customer to Repository and Sql");
                Console.WriteLine("2. Add car to Repository and Sql");
                Console.WriteLine("3. Display all customers from Sql");
                Console.WriteLine("4. Display all cars from Sql");
                Console.WriteLine("5. Display all customers and cars from Sql");
                Console.WriteLine("6. Remove all customers from Sql");
                Console.WriteLine("7. Remove all cars from Sql");
                Console.WriteLine("8. Remove all customers and cars from Sql");
                Console.WriteLine("9. Remove customer by Id from Sql");
                Console.WriteLine("10. Remove car by Id from Sql");
                Console.WriteLine("11. Display file AuditFile.json");
                Console.WriteLine("12. Import data to Sql from file Cars.csv");
                Console.WriteLine("13. Import data to Sql from file Customers.csv");
                Console.WriteLine("14. Create file Customers.json and Car.json from Customers.csv and Car.csv");
                Console.WriteLine("15. Load data from files Customers.json and Cars.json to Sql");
                Console.WriteLine("16. Order data cars from Sql by prices ");
                Console.WriteLine("17. Display cars more expensive than 500.000 from Sql ");
                Console.WriteLine("18. Display customers who can buy cars for the given price ");
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
                            _repositoriesService.AddCustomerToSQL(_customersRepository);                       
                        }
                        break;
                    case "2":
                        {                       
                            _repositoriesService.AddCarToSQL(_carsRepository);                          
                        }
                        break;
                    case "3":
                        {
                            _repositoriesService.WriteAllCustomersToConsole(_customersRepository);                         
                        }
                        break;
                    case "4":
                        {
                            _repositoriesService.WriteAllCarsToConsole(_carsRepository);                      
                        }
                        break;
                    case "5":
                        {                           
                            Console.WriteLine("Cars List:");
                            _repositoriesService.WriteAllCustomersToConsole(_customersRepository);                          
                            Console.WriteLine("Customers List:");
                            _repositoriesService.WriteAllCarsToConsole(_carsRepository);                   
                        }
                        break;
                    case "6":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers - -");
                            _repositoriesService.DeleteAllCustomers(_customersRepository);
                        }
                        break;
                    case "7":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an cars - -");
                            _repositoriesService.DeleteAllCars(_carsRepository);
                        }
                        break;
                    case "8":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers and cars - -");
                            _repositoriesService.DeleteAllCustomers(_customersRepository);
                            _repositoriesService.DeleteAllCars(_carsRepository);
                        }
                        break;
                    case "9":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customer - -");
                            _repositoriesService.RemoveCustomerById(_customersRepository);

                        }
                        break;
                    case "10":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an car - -");
                            _repositoriesService.RemoveCarById(_carsRepository);
                        }
                        break;
                    case "11":
                        {
                            _auditRepository.WriteAllFromAuditFileToConsole();
                        }
                        break;
                    case "12":
                        {
                            Console.WriteLine("Import Data Car From file.csv to Sql:");
                            _repositoriesService.InsertDataCarsToSQLFromCsv(_carsRepository);
                        }
                        break;
                    case "13":
                        {
                            Console.WriteLine("Import Data Customer from File Csv to Sql:");
                            Console.WriteLine("Creating Audit File: Customer.TXT:");
                            _repositoriesService.InsertDataCustomersToSQLFromCsv(_customersRepository);
                        }
                        break;
                    case "14":
                        {
                            Console.WriteLine("Creating file.json from Customer.CSV an Car.CSV:");
                            _jsonServices.ExportToJsonFileSqlRepo();
                        }
                        break;
                    case "15":
                        {
                            Console.WriteLine("Load data from files json:");
                            _jsonServices.LoadDataFromJsonFiles();
                        }
                        break;
                    case "16":
                        {
                            _repositoriesService.OrderCarsByPrices(_carsRepository);
                        }
                        break;
                    case "17":
                        {
                            Console.WriteLine("Cars more expensive than 500.000 SQL ");
                            _repositoriesService.CarsMoreExpensiveThan(_carsRepository);
                        }
                        break;
                    case "18":
                        {
                            Console.WriteLine("18. Customers and the cars they can buy at the given price ");
                            _repositoriesService.GroupCustomersWithCarsByPrice(_customersRepository, _carsRepository);

                        }
                        break;
                    case "19":
                        {
                            Console.WriteLine("19. Cars that a customer can buy based on his Customer Price ");
                            _repositoriesService.DisplayAffordableCarsGroupedByCustomers(_customersRepository,_carsRepository);
                        }
                        break;
                    case "20":
                        {
                            _repositoriesService.ExportCarsGroupedByCustomersToXml(_customersRepository, _carsRepository);
                        }
                        break;
                    case "21":
                        {
                            _repositoriesService.ExportCarsToXml(_carsRepository);                        
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
