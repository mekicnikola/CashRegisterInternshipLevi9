using CashRegister.Application.Services;

namespace CashRegister.Application.Tests
{
    [TestClass]
    public class BillNumberServiceTest
    {
        [TestMethod]
        public void GenerateBillNumber_ReturnsCorrectFormat()
        {
            var billNumberService = new BillNumberService();
            const long identificationCode = 123;
            const long billSequence = 1;

            var result = billNumberService.GenerateBillNumber(identificationCode, billSequence);

            StringAssert.Matches(result, new System.Text.RegularExpressions.Regex(@"^\d{3}-\d{13}-\d{2}$"));
        }

        [TestMethod]
        public void GenerateBillNumber_ReturnsValidControlNumber()
        {
            var billNumberService = new BillNumberService();
            const long identificationCode = 123;
            const long billSequence = 1;

            var result = billNumberService.GenerateBillNumber(identificationCode, billSequence);
            var parts = result.Split('-');
            var controlNumber = int.Parse(parts[2]);
            var baseNumber = long.Parse(parts[0] + parts[1] + "00");

            Assert.AreEqual(98 - (baseNumber % 97), controlNumber);
        }

    }
}
