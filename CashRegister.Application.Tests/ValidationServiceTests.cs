using CashRegister.Application.Services;

namespace CashRegister.Application.Tests
{
    [TestClass]
    public class ValidationServiceTests
    {
        private ValidationService? _validationService;

        [TestInitialize]
        public void SetUp()
        {
            _validationService = new ValidationService();
        }

        [DataTestMethod]
        [DataRow("4111111111111111", true)] // Visa
        [DataRow("4012888888881881", true)] // Visa
        [DataRow("378282246310005", true)] // American Express
        [DataRow("5105105105105100", true)] // MasterCard
        [DataRow("1234567890123456", false)] // invalid number
        [DataRow("4111111111111", false)] // invalid length
        [DataRow(null, false)]
        [DataRow("", false)] 
        public void IsValidCreditCard_ValidatesCorrectly(string creditCardNumber, bool expected)
        {
            var result = _validationService!.IsValidCreditCard(creditCardNumber);
            Assert.AreEqual(expected, result);
        }
    }
}
