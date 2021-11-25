using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using HtmlAgilityPack;
using System.Linq;

using PerandusBacker.Json;

namespace PerandusBacker.Utils
{
  static class Network
  {
    private static HttpClient _httpClient;
    private static HttpClientHandler _httpClientHandler;
    public static readonly Uri PoeUri = new Uri("https://www.pathofexile.com/");
    public static readonly Uri ApiUri = new Uri("https://api.pathofexile.com/");

    private static void EnsureClient()
    {
      if (_httpClient == null)
      {
        CreateClient();
      }
    }

    private static void CreateClient()
    {
      EnsureHandler();
      _httpClient = new HttpClient(_httpClientHandler);
      _httpClient.DefaultRequestHeaders.Add("User-Agent", "Other");
    }

    private static void EnsureHandler()
    {
      if (_httpClientHandler == null)
      {
        CreateHandler();
      }
    }

    private static void CreateHandler()
    {
      HttpClientHandler handler = new HttpClientHandler();
      handler.CookieContainer = new CookieContainer();

      _httpClientHandler = handler;
    }

    public static void UpdatePoeSessionId(string PoeSessionId)
    {
      EnsureHandler();
      Data.PoeSessionId = PoeSessionId;
      _httpClientHandler.CookieContainer.Add(new Cookie("POESESSID", PoeSessionId) { Domain = PoeUri.Host });
    }

    private static async Task<string> Request(Uri uri, string url)
    {
      EnsureClient();

      HttpResponseMessage response = await _httpClient.GetAsync(uri + url);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> RequestPostFormUrl(string url, PostData data)
    {
      return await RequestPostFormUrl(PoeUri, url, data);
    }

    private static async Task<string> RequestPostFormUrl(Uri uri, string url, PostData data)
    {
      EnsureClient();

      FormUrlEncodedContent content = new FormUrlEncodedContent(data.ToArray());

      HttpResponseMessage response = await _httpClient.PostAsync(uri + url, content);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> RequestPostJson(string url, object data)
    {
      return await RequestPostJson(PoeUri, url, data);
    }

    private static async Task<string> RequestPostJson(Uri uri, string url, object data)
    {
      StringContent content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

      HttpResponseMessage response = await _httpClient.PostAsync(uri + url, content);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> Request(string url)
    {
      return await Request(PoeUri, url);
    }

    public static async Task<string> RequestApi(string url)
    {
      return await Request(ApiUri, url);
    }

    public static async Task<bool> AttemptLogin(string PoeSessionId)
    {
      UpdatePoeSessionId(PoeSessionId);
      return await AttemptLogin();
    }

    public static async Task<bool> AttemptLogin()
    {
      try
      {
        string output = await Request("my-account");

        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(output);

        // The first a element contains the accountName
        string accountName = htmlDoc.DocumentNode.SelectSingleNode("//a").InnerText;

        Data.Account.Name = accountName;

        string imageSrc = htmlDoc.DocumentNode.SelectNodes("//img")
          .Where(node => node.GetAttributeValue("alt", "") == "Avatar")
          .First()
          .GetAttributeValue("src", "");

        Data.Account.Image = imageSrc;

        return true;
      }
      catch
      {
        UpdatePoeSessionId("");
        return false;
      }
    }

    public static async Task<string> PostItems(string content, string threadId)
    {
      ForumInfo info = await GetForumInfo(threadId);
      info.Content = content;

      return await RequestPostFormUrl($"forum/edit-thread/{threadId}", info);
    }

    public static async Task<CurrencyPriceResponse> GetCurrencyPrices(Currency currency)
    {
      CurrencyPriceRequest content = Data.CreateCurrencyPriceRequestObject(currency);

      string reqOutput = await RequestPostJson($"api/trade/exchange/{Data.League.Id}", content);
      CurrencyPriceQueryResponse response = JsonSerializer.Deserialize<CurrencyPriceQueryResponse>(reqOutput);

      string req = $"api/trade/fetch/{string.Join(',', response.Result.Take(20))}?query={response.Id}&exchange";
      string output = await Request(req);
      return JsonSerializer.Deserialize<CurrencyPriceResponse>(output);
    }

    private static async Task<ForumInfo> GetForumInfo(string threadId)
    {
      ForumInfo info = new ForumInfo();
      info.Submit = "Submit";

      string output = await Request($"forum/edit-thread/{threadId}");

      HtmlDocument htmlDoc = new HtmlDocument();
      htmlDoc.LoadHtml(output);

      HtmlNodeCollection inputNodes = htmlDoc.DocumentNode.SelectNodes("//input");

      string hash = inputNodes
        .Where(node => node.GetAttributeValue("name", "") == "hash")
        .First()
        .GetAttributeValue("value", "");

      info.Hash = hash;
      info.Title = "TestShop";

      return info;
    }
  }
}
