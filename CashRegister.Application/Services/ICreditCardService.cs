using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegister.Application.Services
{
    public interface ICreditCardService
    {
        Task VerifyUserCreditCardAsync(int userId, string creditCardNumber, int creditCardId);
    }
}
