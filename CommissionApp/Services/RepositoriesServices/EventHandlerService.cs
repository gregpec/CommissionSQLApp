using CommissionApp.Data.Entities;
using CommissionApp.Data.Repositories;

namespace CommissionApp.Services.RepositoriesServices
{
    public class EventHandlerService : IEventHandlerService
    {
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Car> _carsRepository;
      
        public EventHandlerService(IRepository<Customer> customerRepository,
                                   IRepository<Car> carRepository
                                  )
        {
            _customersRepository = customerRepository;
            _carsRepository = carRepository;     
        }

        public void Events()
        {
          
            void TextColoring(ConsoleColor color, string text)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            void CustomerRepositoryOnItemAdded(object? sender, Customer e)
            {
                TextColoring(ConsoleColor.Red, $"Event: Customer {e.FirstName} added to repository => {sender?.GetType().Name}!");
                Console.ForegroundColor = ConsoleColor.Green;
                AddAuditInfo(e, "CUSTOMER ADDED");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Customer\n{e}\nadded successfully to data sql repositories  and event inscribed to file: Resources\\\\Files\\\\Audit.txt\n");
                Console.ResetColor();
            }

            void CustomerRepositoryOnItemRemoved(object? sender, Customer e)
            {
                TextColoring(ConsoleColor.Red, $"Event: Customer {e.FirstName} removed from repository => {sender?.GetType().Name}!");
                AddAuditInfo(e, "CUSTOMER REMOVED");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Customer\n{e}\nremoved successfully  and event inscribed to file: Resources\\Files\\Audit.txtn\n");
                Console.ResetColor();
            }

            void CarRepositoryOnItemAdded(object? sender, Car e)
            {
                TextColoring(ConsoleColor.Red, $"Event: Car {e.CarBrand} added to repository => {sender?.GetType().Name}!");
                Console.ForegroundColor = ConsoleColor.Green;
                AddAuditInfo(e, "CAR ADDED");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Car\n{e}\nadded successfully to data sql repositories and event inscribed to file: Resources\\Files\\Audit.txt\n");
                Console.ResetColor();
            }
            void CarRepositoryOnItemRemoved(object? sender, Car e)
            {
                TextColoring(ConsoleColor.Red, $"Event: Car {e.CarBrand} removed from repository => {sender?.GetType().Name}!");
                AddAuditInfo(e, "CAR REMOVED");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Car\n{e}\nremoved successfully  and event inscribed to file: Resources\\Files\\Audit.txt\n");
                Console.ResetColor();
            }
            _carsRepository.ItemAdded += CarRepositoryOnItemAdded;
            _carsRepository.ItemRemoved += CarRepositoryOnItemRemoved;
            _customersRepository.ItemAdded += CustomerRepositoryOnItemAdded;
            _customersRepository.ItemRemoved += CustomerRepositoryOnItemRemoved;

            void AddAuditInfo<T>(T e, string info) where T : class, IEntity
            {
                using (var writer = File.AppendText(IRepository<IEntity>.auditFileName))
                {
                    writer.WriteLine($"[{DateTime.UtcNow}]\t{info} :\n    [{e}]");
                }
            }

        }
    }
}
