using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
      _httpClientHandler.CookieContainer.Add(new Cookie("POESESSID", PoeSessionId) { Domain = PoeUri.Host });
    }

    private static async Task<string> Request(Uri uri, string url)
    {
      EnsureClient();

      HttpResponseMessage response = await _httpClient.GetAsync(uri + url);
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
        string accountLocation = "/account/view-profile/";
        int startingAccountPosition = output.IndexOf(accountLocation) + accountLocation.Length;
        int endingAccountPosition = output.IndexOf('\"', startingAccountPosition);

        Data.Account.Name = output.Substring(startingAccountPosition, endingAccountPosition - startingAccountPosition);

        string accountImageLocation = "alt=\"Avatar\"";
        int endingAccountImagePosition = output.IndexOf(accountImageLocation) - 2;
        int startingAccountImagePosition = output.LastIndexOf('\"', endingAccountImagePosition - 1) + 1;

        Data.Account.Image = output.Substring(startingAccountImagePosition, endingAccountImagePosition - startingAccountImagePosition);

        return true;
      }
      catch
      {
        UpdatePoeSessionId("");
        return false;
      }
    }
  }
}
