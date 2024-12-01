using CommissionApp.Data.Entities;
using CommissionApp.Data.Repositories;

namespace CommissionApp.Services
{
    public interface IDbContextService
    {
        bool AddCarToSQL(IRepository<Car> carRepository);
        bool AddCustomerToSQL(IRepository<Customer> customerRepository);
        void CarsMoreExpensiveThan();
        void CreateXmL();
        void DeleteAllCars();
        void DeleteAllCustomers();
        void DisplayAffordableCarsGroupedByCustomers();
        void ExportCarsGroupedByCustomersToXml();
        void ExportCarsToXml();
        void ExportToJsonFileSqlRepo();
        void GroupCustomersWithCarsByPrice();
        void InsertDataCustomersToSQLFromCsv();
        void InsertDataToSQLFromCsv();
        void LoadDataFromJsonFiles();
        void OrderCarsByPrices();
        void ReadCarsFromDbSQL();
        void ReadCustomersFromDbSQL();
        bool RemoveCarById();
        bool RemoveCustomerById();
        void TextColoring(ConsoleColor color, string text);
        void WriteAllFromAuditFileToConsole();

    }
}
