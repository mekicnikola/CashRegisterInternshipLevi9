using CashRegister.Application.Services.Dto;

namespace CashRegister.Application.Services
{
    public class CreateBillRequest
    {
        public int UserId { get; set; }
        public string PaymentMethod { get; set; }
        public int? CreditCardId { get; set; }
        public string Currency { get; set; }
        public string CreditCardNumber { get; set; }
        public List<ProductBillDto> Products { get; set; }
    }
}
