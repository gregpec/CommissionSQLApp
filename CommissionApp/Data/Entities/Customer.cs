namespace CommissionApp.Data.Entities
{
    public class Customer : EntityBase
    {
        public Customer(string firstname, string lastname, bool email, decimal price)
        {          
            FirstName=firstname;
            LastName=lastname;
            Email = email;
            Price = price;
        }
        public Customer()
        {
        }
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Email { get; set; }
        public decimal? Price { get; set; }
        public override string ToString() => $"Id: {Id}, FirstName: {FirstName}, LastName: {LastName}, Premium: {Email}, Price EUR: {Price}";
    }
}