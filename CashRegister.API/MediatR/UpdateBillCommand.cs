using CashRegister.Domain.Models;
using MediatR;

namespace CashRegister.Application.Services.MediatR
{
    public class UpdateBillCommand : IRequest<Bill>
    {
        public int BillId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public int? CreditCardId { get; set; }
    }
}
