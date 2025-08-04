using Microsoft.EntityFrameworkCore;
using CommissionApp;
using Microsoft.Extensions.DependencyInjection;
using CommissionApp.Components.CsvReader;
using CommissionApp.Data.Entities;
using CommissionApp.Audit.AuditJsonFile;
using CommissionApp.Data;
using CommissionApp.UserCommunication;
using CommissionApp.Data.Repositories;
using CommissionApp.Services.RepositoriesServices;
using CommissionApp.Services.FilesServices.JsonFile.ExportCsvToJsonFile;
using CommissionApp.Services.FilesServices.JsonFile;
using CommissionApp.Components.DataProviders;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IApp, App>();
        services.AddSingleton<IAudit, JsonAudit>();
        services.AddSingleton<ICarsProvider, CarsProvider>();
        services.AddSingleton<ICsvReader, CsvReader>();
        services.AddSingleton<IJsonFileService<Customer>>(new JsonFileService<Customer>("Resources\\Files\\Customers.json"));
        services.AddSingleton<IJsonFileService<Car>>(new JsonFileService<Car>("Resources\\Files\\Cars.json"));
        services.AddSingleton<IRepository<Customer>, SqlRepository<Customer>>();
        services.AddSingleton<IRepository<Car>, SqlRepository<Car>>();
        services.AddSingleton<IUserCommunication, UserCommunication>();
        services.AddSingleton<IJsonServices, JsonServices>();
        services.AddSingleton<IRepositoriesService, RepositoriesService>();
        services.AddSingleton<IEventHandlerService, EventHandlerService>();
        services.AddDbContext<CommissionAppSQLDbContext>(options => options
        .UseSqlServer("Server=localhost,1433;Database=CarsStorage;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True"));

        //services.AddDbContext<CommissionAppSQLDbContext>(options => options
        //.UseSqlServer("Data Source=LAPTOP-8QEUHJMJ\\SQLEXPRESS;Initial Catalog=\"CarsStorage\";Integrated Security=True;Trust Server Certificate=True"));
        services.AddTransient<IAudit>(provider =>
        {
            string action = "[item Added!]";
            string itemData = "[itemData Added!]";

            return
                 new JsonAudit($"{action}", $"{itemData}");
        });

        var serviceProvider = services.BuildServiceProvider();
        using var dbContext = serviceProvider.GetRequiredService<CommissionAppSQLDbContext>();

try
{
    // Automatyczna migracja (tworzy bazę, jeśli nie istnieje)
    dbContext.Database.Migrate();

    if (dbContext.Database.CanConnect())
    {
        Console.WriteLine("Connection to SQL Server established successfully!!");
        Thread.Sleep(2000);
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
        //using var dbContext = serviceProvider.GetRequiredService<CommissionAppSQLDbContext>();

        /*try   
        {
            if (dbContext.Database.CanConnect())
            {
                Console.WriteLine("Connection to SQL Server established successfully!!");
                Thread.Sleep(2000);
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
        */


        var app = serviceProvider.GetService<IApp>()!;
        app.Run();
    }
}



