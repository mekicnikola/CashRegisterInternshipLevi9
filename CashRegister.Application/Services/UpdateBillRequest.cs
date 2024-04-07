namespace CashRegister.Application.Services
{
    public class UpdateBillRequest
    {
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public int? CreditCardId { get; set; }
    }
}