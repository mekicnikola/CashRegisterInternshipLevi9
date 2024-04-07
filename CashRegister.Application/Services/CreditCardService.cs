using CashRegister.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Application.Services
{
    public class CreditCardService: ICreditCardService
    {
        private readonly CashRegisterDBContext _context;
        private readonly ValidationService _validationService;

        public CreditCardService(CashRegisterDBContext context, ValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task VerifyUserCreditCardAsync(int userId, string creditCardNumber, int creditCardId)
        {
            if (!IsValidCreditCard(creditCardNumber))
            {
                throw new ArgumentException("Credit card number is invalid.");
            }

            var cardExists = await _context.CreditCards.AnyAsync(cc => cc.UserId == userId && cc.Id == creditCardId);

            if (!cardExists)
            {
                throw new ArgumentException("Credit card number does not match the user's card.");
            }

            var cardNumberMatches = await _context.CreditCards.AnyAsync(cc => cc.Id == creditCardId && cc.Number == creditCardNumber);

            if (!cardNumberMatches)
            {
                throw new ArgumentException("Credit card number does not match the user's card.");
            }
        }

        public bool IsValidCreditCard(string creditCardNumber)
        {
            return _validationService.IsValidCreditCard(creditCardNumber);
        }
    }
}
