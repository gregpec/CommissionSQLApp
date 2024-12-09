using CommissionApp.Audit.AuditJsonFile;
using CommissionApp.Components.CsvReader;
using CommissionApp.Data.Entities;
using CommissionApp.Services.FilesServices.JsonFile.ExportCsvToJsonFile;

namespace CommissionApp.Services.FilesServices.JsonFile
{
    public class JsonServices : IJsonServices
    {
      
        private readonly ICsvReader _csvReader;
        private readonly IJsonFileService<Customer> _jsonCustomerService;
        private readonly IJsonFileService<Car> _jsonCarService;
        private readonly IAudit _auditRepository;


        public JsonServices
            (
               ICsvReader csvReader,
               IJsonFileService<Customer> jsonCustomerService,
               IJsonFileService<Car> jsonCarService,
               IAudit auditRepository
            )
        {
            _csvReader = csvReader;
            _jsonCustomerService = jsonCustomerService;
            _jsonCarService = jsonCarService;
            _auditRepository = auditRepository;
         
        }

        public void ExportToJsonFileSqlRepo()
        {
            string action = "Converting Csv To Json file";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            
            var customRecords = _csvReader.ProcessCustomers("Resources\\Files\\Customers.csv");
            _jsonCustomerService.SaveToFile(customRecords);

            var records = _csvReader.ProcessCars("Resources\\Files\\Cars.csv");
            _jsonCarService.SaveToFile(records);

            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
            Console.WriteLine("Customers and Cars have been successfully exported to JSON.");
        }
        public void LoadDataFromJsonFiles()
        {
            string action = "Load data from jsom to console";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
            var customerFromFile = _jsonCustomerService.LoadFromFile();
            var carFromFile = _jsonCarService.LoadFromFile();

            if (customerFromFile.Any())
            {
                foreach (var customer in customerFromFile)
                {
                    Console.WriteLine($"ID: {customer.Id}, FirstName: {customer.FirstName}, LastName: {customer.LastName}, Premium: {customer.Email}, Price: {customer.Price:C}");
                }
            }
            else
            {
                Console.WriteLine("No files json");
            }
            if (carFromFile.Any())
            {
                foreach (var car in carFromFile)
                {
                    Console.WriteLine($"ID: {car.Id}, Brand: {car.CarBrand}, Model: {car.CarModel}, Price: {car.CarPrice:C}");
                }
            }
            else
            {
                Console.WriteLine("No files json");
            }
        }
    }
}
