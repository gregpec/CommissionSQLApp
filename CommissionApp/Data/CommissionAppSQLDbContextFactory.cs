using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using CommissionApp.Data.Entities;

namespace CommissionApp.Data;

    public class CommissionAppSQLDbContextFactory : IDesignTimeDbContextFactory<CommissionAppSQLDbContext>
    {
        public CommissionAppSQLDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CommissionAppSQLDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=CarsStorage;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True");

            return new CommissionAppSQLDbContext(optionsBuilder.Options);
        }
    }

