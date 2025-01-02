using Microsoft.EntityFrameworkCore;
using CommissionApp.Data.Entities;

namespace CommissionApp.Data;
public class CommissionAppSQLDbContext : DbContext
{
        public CommissionAppSQLDbContext(DbContextOptions<CommissionAppSQLDbContext> options)
            : base(options)
        {
        }
        public DbSet<Car> Cars { get; set;  }
        public DbSet<Customer> Customers { get; set; }
}
