namespace CashRegister.Domain.Models
{
    public class Bill
    {
        public int Id { get; set; }

        public string Number { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public int? CreditCardId { get; set; }
        public CreditCard? CreditCard { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<ProductBill> ProductBills { get; set; } = new();
    }

}
