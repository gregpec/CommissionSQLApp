using CommissionApp.Data.Entities;
using CommissionApp.Data.Repositories;

namespace CommissionApp.Services
{
    public class EventHandlerService: IEventHandlerService
    {
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Car> _carsRepository;
       
        public EventHandlerService(IRepository<Customer> customerRepository, 
                                   IRepository<Car> carRepository)
                                          
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

            void CustomerRepositoryAdded(object? sender, Customer e)
            {           
                TextColoring(ConsoleColor.Red, $"Event: Customer Event {e.FirstName} added from repository => {sender?.GetType().Name}!");
                Console.WriteLine($"Customer\n{e}\nadded successfully.\n");
                Console.ResetColor();

            }
         
             void CarRepositoryOnItemAdded(object? sender, Car e)
            {
                TextColoring(ConsoleColor.Red, $"Event: Car {e.CarBrand} added from repository => {sender?.GetType().Name}!");
            }

            _customersRepository.ItemAdded += CustomerRepositoryAdded;       
            _carsRepository.ItemAdded += CarRepositoryOnItemAdded;

        }
    }
}
