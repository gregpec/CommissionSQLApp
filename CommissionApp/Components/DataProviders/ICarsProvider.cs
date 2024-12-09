using CommissionApp.Data.Entities;

namespace CommissionApp.Components.DataProviders
{
    public interface ICarsProvider
    {
        public Car GetCarWithMinimumPrice();
        public decimal GetMinimumPriceOfAllCars();
        public List<Car> OrderByName();
        public List<Car> GetCarsSortedByPrice();
        public List<Car> WhereStartsWithAndCostIsGraterThan(string prefix, decimal cost);
        public List<string> DistinctAllCarModel();
        public List<Car> DistinctByPrice();
    }
}
