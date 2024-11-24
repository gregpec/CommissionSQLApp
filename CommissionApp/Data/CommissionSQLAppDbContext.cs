using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;

using CommissionApp;
using CommissionApp.Data.Entities;
namespace CommissionApp.Data;
public class CommissionAppSQLDbContext : DbContext
{
  
        public CommissionAppSQLDbContext(DbContextOptions<CommissionAppSQLDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Customer> Customers { get; set; }



    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.LogTo(Console.WriteLine);  // logi

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("YourConnectionString")
    //                  .LogTo(Console.WriteLine);
    //}

}
