using FluentValidation;

namespace CashRegister.Application.Services
{
    public class CreateBillRequestValidator : AbstractValidator<CreateBillRequest>
    {


        public CreateBillRequestValidator()
        {
            RuleFor(request => request.UserId).NotEmpty().WithMessage("User ID is required.");

            When(request => request.PaymentMethod == "Card", () =>
            {
                RuleFor(request => request.CreditCardNumber)
                    .NotEmpty().WithMessage("Credit card number is required for card payments.");
            });

            RuleFor(request => request.Currency)
                .Must(BeAValidCurrency)
                .WithMessage("Currency must be EUR, USD, or RSD.");
        }

        private static bool BeAValidCurrency(string currency)
        {
            var validCurrencies = new[] { "EUR", "USD", "RSD" };
            return validCurrencies.Contains(currency);
        }
    }
}
