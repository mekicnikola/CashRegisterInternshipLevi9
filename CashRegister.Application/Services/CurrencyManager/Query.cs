using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services.CurrencyManager
{
    public class Query
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
    }

}
