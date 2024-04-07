using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CashRegister.Application.Services.CurrencyManager
{
    public class CurrencyConversionResponse
    {
        public bool Success { get; set; }
        public Query Query { get; set; }
        public Info Info { get; set; }
        public string Date { get; set; }
        public decimal Result { get; set; }
    }
}
