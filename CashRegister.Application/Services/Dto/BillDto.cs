using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services.Dto
{
    public class BillDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductBillDto> ProductBills { get; set; } = new List<ProductBillDto>();
    }
}
