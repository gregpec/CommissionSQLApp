using CommissionApp.Data.Entities;
using CommissionApp.Data.Repositories;

namespace CommissionApp.Services
{
    public interface IDbContextService
    {
        bool AddCarToSQL(IRepository<Car> carRepository);
        bool AddCustomerToSQL(IRepository<Customer> customerRepository);
        void WriteAllCarsToConsole(IRepository<Car> carRepository);
        void CarsMoreExpensiveThan(IRepository<Car> carRepository);
        void DeleteAllCars(IRepository<Car> carRepository);
        void DeleteAllCustomers(IRepository<Customer> customerRepository);
        void DisplayAffordableCarsGroupedByCustomers(IRepository<Customer> customerRepository, IRepository<Car> carRepository);
        void ExportCarsGroupedByCustomersToXml(IRepository<Customer> customerRepository, IRepository<Car> carRepositor);
        void ExportCarsToXml(IRepository<Car> carRepository);
        void ExportToJsonFileSqlRepo();
        void GroupCustomersWithCarsByPrice(IRepository<Customer> customerRepository, IRepository<Car> carRepository);
        void InsertDataCustomersToSQLFromCsv(IRepository<Customer> customerRepository);
        void InsertDataCarsToSQLFromCsv(IRepository<Car> customerRepository);
        void LoadDataFromJsonFiles();
        void OrderCarsByPrices(IRepository<Car> carRepository);     
        void RemoveCarById(IRepository<Car> carRepository);
        void RemoveCustomerById(IRepository<Customer> customerRepository);
        void TextColoring(ConsoleColor color, string text);
        void WriteAllFromAuditFileToConsole();
        void WriteAllCustomersToConsole(IRepository<Customer> customerRepository);

    }
}
