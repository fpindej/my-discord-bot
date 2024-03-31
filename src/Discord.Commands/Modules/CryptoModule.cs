using CoinApi.Client;
using Discord.Interactions;

namespace Discord.Commands.Modules;

public sealed class CryptoModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ICryptocurrencyExchangeRateProvider _exchangeRateProvider;

    public CryptoModule(ICryptocurrencyExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    [SlashCommand("cryptoexchange", "Get exchange rate")]
    
    public async Task GetExchangeRate(Cryptocurrency cryptocurrency, FiatCurrency fiatCurrency)
    {
        await DeferAsync();
        var rate = await _exchangeRateProvider.GetExchangeRateAsync(cryptocurrency.ToString(), fiatCurrency.ToString());
        await FollowupAsync($"1 {rate.CryptoCurrency} = {rate.Rate:f2} {rate.FiatCurrency}");
    }
}

public enum Cryptocurrency
{
    BTC,
    ETH,
    ADA,
    DOGE
}

public enum FiatCurrency
{
    USD,
    EUR,
    GBP,
    CZK
}