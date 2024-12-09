namespace CommissionApp.Services.RepositoriesServices
{
    public interface IRepositoriesService
    {
        bool AddCarToSQL();
        bool AddCustomerToSQL();
        void WriteAllCarsToConsole();
        void CarsMoreExpensiveThan();
        void DeleteAllCars();
        void DeleteAllCustomers();
        void DisplayAffordableCarsGroupedByCustomers();
        void ExportCarsGroupedByCustomersToXml();
        void ExportCarsToXml();      
        void GroupCustomersWithCarsByPrice();
        void InsertDataCustomersToSQLFromCsv();
        void InsertDataCarsToSQLFromCsv();
        void OrderCarsByPrices();
        void RemoveCarById();
        void RemoveCustomerById();
        void TextColoring(ConsoleColor color, string text);
        void WriteAllCustomersToConsole();
        void CreateXmL();

    }
}
