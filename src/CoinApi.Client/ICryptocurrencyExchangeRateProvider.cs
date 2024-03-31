namespace CoinApi.Client;

public interface ICryptocurrencyExchangeRateProvider
{
    Task<CryptocurrencyExchangeRate> GetExchangeRateAsync(string cryptoCurrency, string fiatCurrency);
}

