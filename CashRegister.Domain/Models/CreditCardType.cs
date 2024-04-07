namespace CashRegister.Domain.Models
{
    public class CreditCardType
    {
        public int Id { get; set; }
        public string CardTypeName { get; set; }
        public List<CreditCard> CreditCards { get; set; }
    }
}
