namespace CashRegister.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } // Encrypt
        public List<UserRole> UserRoles { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        public List<Bill> Bills { get; set; }
    }

}
