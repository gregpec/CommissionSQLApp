namespace CommissionApp.Services
{
    public interface IDbContextService
    {
        bool AddCustomerToSQL();
        bool RemoveCustomerById();
        bool AddCarToSQL();
        bool RemoveCarById();
        void DeleteAllCustomers();
        void DeleteAllCars();
        void ReadCarsFromDbSQL();
        void ReadCustomersFromDbSQL();
        void WriteAllFromAuditFileToConsole();
        void TextColoring(ConsoleColor color, string text);
        void InsertDataToSQLFromCsv();
        void InsertDataCustomersToSQLFromCsv();
        void GroupCustomersWithCarsByPrice();
        void DisplayAffordableCarsGroupedByCustomers();
        void CreateXmL();
        void ExportCarsToXml();
        void ExportCarsGroupedByCustomersToXml();
        void LoadDataFromJsonFiles();
        void ExportToJsonFileSqlRepo();
        void OrderCarsByPrices();
        void CarsMoreExpensiveThan();
    }
}
