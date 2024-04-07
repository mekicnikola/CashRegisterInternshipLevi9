namespace CashRegister.Application.Services
{
    public class ValidationService
    {
        public bool IsValidCreditCard(string creditCard)
        {
            bool isValid;
            if (creditCard == null)
            {
                isValid = false;
                return isValid;
            }
            if (creditCard.Length != 13 && creditCard.Length != 15 && creditCard.Length != 16)
            {
                isValid = false;
            }
            else
            {
                switch (creditCard.Length)
                {
                    case 13 or 16 when creditCard.StartsWith('4'):
                    case 15 when (creditCard.StartsWith("34") || creditCard.StartsWith("37")):
                    case 16 when (creditCard.StartsWith("51") || creditCard.StartsWith("52") || creditCard.StartsWith("53")
                                  || creditCard.StartsWith("54") || creditCard.StartsWith("55")):
                        isValid = ValidateCreditCard(creditCard);
                        break;
                    default:
                        isValid = false;
                        break;
                }
            }
            return isValid;
        }
        private static bool ValidateCreditCard(string number)
        {
            var sum = 0;
            var alternate = false;

            for (var i = number.Length - 1; i >= 0; i--)
            {
                var digit = int.Parse(number[i].ToString());

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }
                sum += digit;
                alternate = !alternate;
            }

            return (sum % 10 == 0);
        }

    }
}
