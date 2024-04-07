using CashRegister.Application.Services.CurrencyManager;
using CashRegister.Domain.Models;

namespace CashRegister.Application.Services
{
    public class BillManager
    {
        private readonly List<Bill> _bills = new();
        private Bill _currentBill;
        private readonly ICurrencyManager _currencyManager;
        private readonly string _currency;
        private decimal _currentTotalPrice;
        private const decimal LimitInRsd = 30000;
        private readonly BillNumberService _billNumberService;
        private long _billSequence = 1;

        public BillManager(int userId, string paymentMethod, int? creditCardId, ICurrencyManager currencyManager, string currency, BillNumberService billNumberService)
        {
            _currencyManager = currencyManager;
            _currency = currency;
            _billNumberService = billNumberService;
            StartNewBill(userId, paymentMethod, creditCardId);
        }
        public async Task AddProductToBill(Product product, int quantity)
        {
            decimal productPriceInRsd;
            if (_currency.Equals("RSD"))
            {
                productPriceInRsd = product.Price;
            }
            else
            {
                productPriceInRsd = await _currencyManager.ConvertCurrencyAsync("RSD", _currency, product.Price);
            }

            var totalPriceForProduct = productPriceInRsd * quantity;

            if (_currentTotalPrice + totalPriceForProduct > LimitInRsd && _currentBill.ProductBills.Any())
            {
                FinishCurrentBill();
                StartNewBill(_currentBill.UserId, _currentBill.PaymentMethod, _currentBill.CreditCardId);
            }

            decimal priceForDisplay;
            if (!_currency.Equals("RSD"))
            {
                priceForDisplay = await _currencyManager.ConvertCurrencyAsync("RSD", _currency, totalPriceForProduct);
            }
            else
            {
                priceForDisplay = totalPriceForProduct;
            }

            _currentBill.ProductBills.Add(new ProductBill
            {
                ProductId = product.Id,
                Quantity = quantity,
                Price = priceForDisplay
            });

            _currentTotalPrice += totalPriceForProduct;
        }

        private void StartNewBill(int userId, string paymentMethod, int? creditCardId)
        {
            var identificationCode = GenerateIdentificationCodeBasedOnDateTime();
            var billNumber = _billNumberService.GenerateBillNumber(identificationCode, _billSequence);

            _currentBill = new Domain.Models.Bill
            {
                UserId = userId,
                PaymentMethod = paymentMethod,
                CreditCardId = paymentMethod == "Card" ? creditCardId : null,
                CreatedAt = DateTime.UtcNow,
                ProductBills = new List<ProductBill>(),
                Number = billNumber
            };
            _currentTotalPrice = 0;
            _billSequence++;
        }

        public long GenerateIdentificationCodeBasedOnDateTime()
        {
            var now = DateTime.Now;

            var dayOfYear = now.DayOfYear;
            var hour = now.Hour;
            var minute = now.Minute;
            var second = now.Second;

            long code = dayOfYear * (hour + 1) * (minute + 1) * (second + 1) % 900 + 100;

            return code;
        }

        private void FinishCurrentBill()
        {
            _currentBill.TotalPrice = _currentTotalPrice;
            _bills.Add(_currentBill);
        }

        public List<Bill> FinishBilling()
        {
            if (_currentBill.ProductBills.Any())
            {
                FinishCurrentBill();
            }
            return _bills;
        }
    }

}
