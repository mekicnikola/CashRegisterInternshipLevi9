namespace CashRegister.Domain.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int TypeId { get; set; }
        public CreditCardType CreditCardType { get; set; }
        public string Number { get; set; } //encrypt
        public DateTime ExpirationDate { get; set; }
        public List<Bill> Bills { get; set; }
    }
}
