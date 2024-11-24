using System.Globalization;
using CommissionApp.Data.Entities;

namespace CommissionApp.Components.CsvReader.Extensions
{
    public static class CustomerExtensions
    {
        public static IEnumerable<Customer> ToCustomer(this IEnumerable<string> source) 
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                yield return new Customer
                {                  
                    FirstName = columns[0],
                    LastName = columns[1],
                    Email = bool.Parse(columns[2]), 
                    Price= decimal.Parse(columns[3], CultureInfo.InvariantCulture),
                };
            }
        }
    }
}
