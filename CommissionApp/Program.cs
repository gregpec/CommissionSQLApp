using Microsoft.EntityFrameworkCore;
using CommissionApp;
using Microsoft.Extensions.DependencyInjection;
using CommissionApp.Components.CsvReader;
using CommissionApp.Data.Entities;
using CommissionApp.Audit.InputToSqlAuditTxtFile;
using CommissionApp.JsonFile.ImportCsvToSqlExportJsonFile;
using CommissionApp.Data;
using CommissionApp.Services;
using CommissionApp.UserCommunication;

var services = new ServiceCollection();

services.AddSingleton<IApp, App>();
services.AddSingleton<ICsvReader, CsvReader>();
services.AddSingleton<IJsonFileService<Customer>>(new JsonFileService<Customer>("Resources\\Files\\Customers.json"));
services.AddSingleton<IJsonFileService<Car>>(new JsonFileService<Car>("Resources\\Files\\Cars.json"));
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IDbContextService, DbContextService>();
services.AddDbContext<CommissionAppSQLDbContext>(options => options
.UseSqlServer("Data Source=LAPTOP-8QEUHJMJ\\SQLEXPRESS;Initial Catalog=\"CarsStorage\";Integrated Security=True;Trust Server Certificate=True"));

var options = new DbContextOptionsBuilder<CommissionAppSQLDbContext>()
    .UseSqlServer("Data Source=LAPTOP-8QEUHJMJ\\SQLEXPRESS;Initial Catalog=\"CarsStorage\";Integrated Security=True;Trust Server Certificate=True")
    .Options;

using var dbContext = new CommissionAppSQLDbContext(options);

try
{
    if (dbContext.Database.CanConnect())
    {
        Console.WriteLine("Connection to SQL Server established successfully!!");
    }
    else
    {
        Console.WriteLine("Failed to connect to the database");
        return;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to connect to SQL Server: {ex.Message}");
    return;
}

services.AddTransient<IAudit>(provider =>
{
    string action = "[item Added!]";
    string itemData = "[itemData Added!]";

    return
         new JsonAudit($"{action}", $"{itemData}");
});

var serviceProvider = services.BuildServiceProvider(); 
var app = serviceProvider.GetService<IApp>()!;

app.Run(); 

