using System.Text.Json;

namespace CashRegister.Application.Services.CurrencyManager
{
    public class CurrencyManager: ICurrencyManager
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "328b1ca496mshaffecef2aed38aap11d2d7jsnb0255494c8a5";

        public CurrencyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var requestUrl = $"https://currency-conversion-and-exchange-rates.p.rapidapi.com/convert?from={fromCurrency}&to={toCurrency}&amount={amount}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("X-RapidAPI-Key", ApiKey);
            request.Headers.Add("X-RapidAPI-Host", "currency-conversion-and-exchange-rates.p.rapidapi.com");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var conversionResult = JsonSerializer.Deserialize<CurrencyConversionResponse>(content, options);

            if (conversionResult?.Success == true)
            {
                return conversionResult.Result;
            }

            throw new ApplicationException($"Failed to convert currency from {fromCurrency} to {toCurrency}. Response: {content}");
        }
    }
}
