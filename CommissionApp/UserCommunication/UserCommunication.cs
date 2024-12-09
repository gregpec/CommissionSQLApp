using CommissionApp.Services.RepositoriesServices;
using CommissionApp.Services.FilesServices.JsonFile;
using CommissionApp.Audit.AuditJsonFile;
using CommissionApp.Components.DataProviders;

namespace CommissionApp.UserCommunication
{
    public class UserCommunication : IUserCommunication
    {
        private readonly IRepositoriesService _repositoriesService;
        private readonly IEventHandlerService _eventHandlerService;
        private readonly IJsonServices _jsonServices;
        private readonly IAudit _auditRepository;
        private readonly ICarsProvider _carsProvider;

        public UserCommunication(
                                IRepositoriesService repositoriesService,
                                IEventHandlerService eventHandlerService,
                                IJsonServices jsonServices,
                                IAudit auditRepository,
                                ICarsProvider carsProvider
            )
        {
          
            _repositoriesService = repositoriesService;
            _eventHandlerService = eventHandlerService;
            _jsonServices = jsonServices;
            _auditRepository = auditRepository;
            _carsProvider = carsProvider;
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
                Console.WriteLine("             1 to order data cars by prices ");
                Console.WriteLine("             2 to distinct all cars by model ");
                Console.WriteLine("             3 to get minimum price of all cars  ");
                Console.WriteLine("             4 to create file cars.XmL ");
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
                            _repositoriesService.AddCustomerToSQL();                       
                        }
                        break;
                    case "2":
                        {                       
                            _repositoriesService.AddCarToSQL();                          
                        }
                        break;
                    case "3":
                        {
                            _repositoriesService.WriteAllCustomersToConsole();                         
                        }
                        break;
                    case "4":
                        {
                            _repositoriesService.WriteAllCarsToConsole();                      
                        }
                        break;
                    case "5":
                        {                           
                            Console.WriteLine("Cars List:");
                            _repositoriesService.WriteAllCustomersToConsole();                          
                            Console.WriteLine("Customers List:");
                            _repositoriesService.WriteAllCarsToConsole();                   
                        }
                        break;
                    case "6":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers - -");
                            _repositoriesService.DeleteAllCustomers();
                        }
                        break;
                    case "7":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an cars - -");
                            _repositoriesService.DeleteAllCars();
                        }
                        break;
                    case "8":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customers and cars - -");
                            _repositoriesService.DeleteAllCustomers();
                            _repositoriesService.DeleteAllCars();
                        }
                        break;
                    case "9":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an customer - -");
                            _repositoriesService.RemoveCustomerById();

                        }
                        break;
                    case "10":
                        {
                            _repositoriesService.TextColoring(ConsoleColor.DarkGreen, "\n- - Removing an car - -");
                            _repositoriesService.RemoveCarById();
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
                            _repositoriesService.InsertDataCarsToSQLFromCsv();
                            {
                                string submenu;
                                Console.WriteLine("Imort Data Car From file.csv to Sql:");
                                
                                do
                                {
                                    Console.WriteLine();
                                    Console.WriteLine("\n================ Data Cars ================");
                                    Console.WriteLine("1 to order data cars by prices ");
                                    Console.WriteLine("2 to distinct all cars by model ");
                                    Console.WriteLine("3 to get minimum price of all cars  ");
                                    Console.WriteLine("4 to order by cars name  ");
                                    Console.WriteLine("5 to create file cars.XmL ");
                                    Console.WriteLine(" Press q to exit program: ");
                                    submenu = Console.ReadLine();

                                    switch (submenu)
                                    {
                                        case "1":
                                            {
                                                Console.WriteLine("OrderByPrice");

                                                foreach (var item in _carsProvider.GetCarsSortedByPrice())
                                                {
                                                    Console.WriteLine(item);
                                                }
                                            }
                                            break;
                                        case "2":
                                            {
                                                Console.WriteLine("DistinctAllCarModel ");
                                                foreach (var item in _carsProvider.DistinctAllCarModel())
                                                {
                                                    Console.WriteLine(item);
                                                }
                                            }
                                            break;
                                        case "3":
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine($"Minimum Price Of All Cars: {_carsProvider.GetMinimumPriceOfAllCars()}, ");
                                                Console.WriteLine($"Car With Minimum Price:{_carsProvider.GetCarWithMinimumPrice()},");
                                            }
                                            break;                                        
                                        case "4":
                                            {
                                                Console.WriteLine("Order by Name  ");
                                                _carsProvider.OrderByName();
                                               
                                            }
                                            break;
                                        case "5":
                                            {
                                                Console.WriteLine("CreateXmL  ");
                                                _repositoriesService.CreateXmL();                                          
                                            }
                                            break;
                                      
                                        default:
                                            Console.WriteLine("Invalid input. Please try again.");
                                            break;
                                    }
                                } while (submenu != "q");
                                break;
                            }
                            break;
                        }
                        break;
                    case "13":
                        {
                            Console.WriteLine("Import Data Customer from File Csv to Sql:");
                            Console.WriteLine("Creating Audit File: Customer.TXT:");
                            _repositoriesService.InsertDataCustomersToSQLFromCsv();
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
                            _repositoriesService.OrderCarsByPrices();
                        }
                        break;
                    case "17":
                        {
                            Console.WriteLine("Cars more expensive than 500.000 SQL ");
                            _repositoriesService.CarsMoreExpensiveThan();
                        }
                        break;
                    case "18":
                        {
                            Console.WriteLine("18. Customers and the cars they can buy at the given price ");
                            _repositoriesService.GroupCustomersWithCarsByPrice();

                        }
                        break;
                    case "19":
                        {
                            Console.WriteLine("19. Cars that a customer can buy based on his Customer Price ");
                            _repositoriesService.DisplayAffordableCarsGroupedByCustomers();
                        }
                        break;
                    case "20":
                        {
                            _repositoriesService.ExportCarsGroupedByCustomersToXml();
                        }
                        break;
                    case "21":
                        {
                            _repositoriesService.ExportCarsToXml();                        
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
