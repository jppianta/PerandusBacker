using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PerandusBacker.Json
{
  public class CurrencyPriceInfo
  {
    [JsonPropertyName("chaosPrice")]
    public double ChaosPrice { get; set; }

    [JsonPropertyName("timestamp")]
    public long TimeStamp { get; set; }
  }
  public class ItemPriceInfo
  {
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("amount")]
    public double Amount { get; set; }
  }

  internal class ItemPrice
  {
    [JsonPropertyName("item")]
    public ItemPriceInfo Item { get; set; }

    [JsonPropertyName("exchange")]
    public ItemPriceInfo Exchange { get; set; }
  }

  internal class CurrencyPriceListing
  {
    [JsonPropertyName("price")]
    public ItemPrice Price { get; set; }
  }

  internal class CurrencyPriceResponseListing
  {
    [JsonPropertyName("listing")]
    public CurrencyPriceListing Listing { get; set; }
  }

  internal class CurrencyPriceResponse
  {
    [JsonPropertyName("result")]
    public CurrencyPriceResponseListing[] Result { get; set; }
  }

  internal class CurrencyPriceQueryResponse
  {
    [JsonPropertyName("result")]
    public string[] Result { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
  }

  internal class CurrencyPriceStatusOptions
  {
    [JsonPropertyName("option")]
    public string Option { get; set; }
  }

  internal class CurrencyPriceOptions
  {
    [JsonPropertyName("status")]
    public CurrencyPriceStatusOptions Status { get; set; }

    [JsonPropertyName("have")]
    public string[] Have { get; set; }

    [JsonPropertyName("want")]
    public string[] Want { get; set; }
  }

  internal class CurrencyPriceRequest
  {
    [JsonPropertyName("exchange")]
    public CurrencyPriceOptions Exchange { get; set; }
  }

  internal class Prices {
    [JsonPropertyName("items")]
    public Dictionary<string, ItemPriceInfo> Items { get; set; }

    [JsonPropertyName("currencies")]
    public Dictionary<string, CurrencyPriceInfo> Currencies { get; set; }
  }
}
