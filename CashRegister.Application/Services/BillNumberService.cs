namespace CashRegister.Application.Services
{
    public class BillNumberService
    {
        public string GenerateBillNumber(long identificationCode, long billSequence)
        {
            var basePart = $"{identificationCode:D3}{billSequence:D13}";
            var baseNumber = long.Parse(basePart);
            var controlNumberCalculation = (baseNumber * 100) % 97;
            var controlNumber = 98 - (int)controlNumberCalculation;

            return $"{identificationCode:D3}-{billSequence:D13}-{controlNumber:D2}";
        }
    }
}
