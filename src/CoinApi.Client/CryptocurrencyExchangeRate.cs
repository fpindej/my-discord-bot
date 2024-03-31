namespace CoinApi.Client;

public class CryptocurrencyExchangeRate
{
    public string CryptoCurrency { get; init; } = null!;
    
    public string FiatCurrency { get; init; } = "EUR";
    
    public decimal Rate { get; init; }
}