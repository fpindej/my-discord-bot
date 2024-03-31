using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace CoinApi.Client;

public class CryptocurrencyExchangeRateProvider : ICryptocurrencyExchangeRateProvider
{
    private readonly ILogger<ICryptocurrencyExchangeRateProvider> _logger;
    private readonly HttpClient _httpClient;
    
    private const string Endpoint = "/v1/exchangerate";

    public CryptocurrencyExchangeRateProvider(HttpClient httpClient,
        ILogger<ICryptocurrencyExchangeRateProvider> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    

    public async Task<CryptocurrencyExchangeRate> GetExchangeRateAsync(string cryptocurrency, string fiatCurrency)
    {
        var request = await _httpClient.GetAsync($"{Endpoint}/{cryptocurrency}/{fiatCurrency}");
        var response = await request.Content.ReadAsStringAsync();

        if (IsResponseInvalid(request))
        {
            _logger.LogError("Failed to get exchange rate for {Symbol}. Response: {Response}", cryptocurrency, response);
            throw new Exception("Failed to get exchange rate.");
        }
        
        var rate = JsonSerializer.Deserialize<CoinApiRateResponse>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        if (rate is null)
        {
            _logger.LogError("Failed to deserialize response for {Symbol}. Response: {Response}", cryptocurrency, response);
            throw new Exception("Failed to get exchange rate.");
        }
        
        return new CryptocurrencyExchangeRate
        {
            CryptoCurrency = rate.AssetIdBase,
            FiatCurrency = rate.AssetIdQuote,
            Rate = rate.Rate
        };
    }
    
    private static bool IsResponseInvalid(HttpResponseMessage resp)
        => resp.StatusCode is HttpStatusCode.BadRequest
            or HttpStatusCode.Unauthorized
            or HttpStatusCode.PaymentRequired
            or HttpStatusCode.Forbidden
            or HttpStatusCode.TooManyRequests
            or HttpStatusCode.InternalServerError;

    private class CoinApiRateResponse
    {
        [JsonPropertyName("asset_id_base")]
        public string AssetIdBase { get; init; } = null!;
        
        [JsonPropertyName("asset_id_quote")]
        public string AssetIdQuote { get; init; } = null!;
        
        [JsonPropertyName("rate")]
        public decimal Rate { get; init; }
    }
}