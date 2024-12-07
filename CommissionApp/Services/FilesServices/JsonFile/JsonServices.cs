using CommissionApp.Audit.AuditJsonFile;
using CommissionApp.Components.CsvReader;
using CommissionApp.Data.Entities;
using CommissionApp.Data.Repositories;
using CommissionApp.Data;
using CommissionApp.Services.FilesServices.JsonFile.ExportCsvToJsonFile;
using CommissionApp.Services.RepositoriesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommissionApp.Services.FilesServices.JsonFile
{

    public class JsonServices : IJsonServices
    {
        private readonly CommissionAppSQLDbContext _commissionAppSQLDbContext;
        private readonly ICsvReader _csvReader;
        private readonly IJsonFileService<Customer> _jsonCustomerService;
        private readonly IJsonFileService<Car> _jsonCarService;
        private readonly IAudit _auditRepository;


        public JsonServices
            (CommissionAppSQLDbContext commissionAppSQLDbContext,
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
            _commissionAppSQLDbContext = commissionAppSQLDbContext;
            _commissionAppSQLDbContext.Database.EnsureCreated();
        }

        public void ExportToJsonFileSqlRepo()
        {
            string action = "Converting Csv To Json file";
            string itemData = "!";
            var auditRepository = new JsonAudit($"{action}", $"{itemData}");
            string filePath = "Resources\\Files\\Customers.csv";
            var customRecords = _csvReader.ProcessCustomers("Resources\\Files\\Customers.csv");
            List<Customer> customers = _csvReader.ProcessCustomers(filePath);

            foreach (var customer in customRecords)
            {
                _jsonCustomerService.SaveToFile(_commissionAppSQLDbContext.Customers);          
            }
            string file = "Resources\\Files\\Cars.csv";
            var records = _csvReader.ProcessCars("Resources\\Files\\Cars.csv");

            List<Car> cars = _csvReader.ProcessCars(file);
            foreach (var car in records)
            {
              
                _jsonCarService.SaveToFile(_commissionAppSQLDbContext.Cars);
            }
            foreach (var car in records)
            {
                Console.WriteLine($"{car}");
            }
            auditRepository.AddEntryToFile();
            auditRepository.SaveAuditFile();
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
