using System.Net;
using System.Net.Http.Headers;

namespace BumsBot
{

    public class BumsApi
    {
        private readonly HttpClient client;

        public BumsApi(int Mode, string queryID, int queryIndex, ProxyType[] Proxy)
        {
            var FProxy = Proxy.Where(x => x.Index == queryIndex);
            if (FProxy.Count() != 0)
            {
                if (!string.IsNullOrEmpty(FProxy.ElementAtOrDefault(0)?.Proxy))
                {
                    var handler = new HttpClientHandler() { Proxy = new WebProxy() { Address = new Uri(FProxy.ElementAtOrDefault(0)?.Proxy ?? string.Empty) } };
                    client = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, 30) };
                }
                else
                    client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            }
            else
                client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true, NoStore = true };
            if (Mode == 1)
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {queryID}");
            client.DefaultRequestHeaders.Add("Accept-Language", "en");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Origin", "https://app.bums.bot");
            client.DefaultRequestHeaders.Add("Referer", "https://app.bums.bot/");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
            client.DefaultRequestHeaders.Add("User-Agent", Tools.getUserAgents(queryIndex));
            client.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?1");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", $"\"{Tools.getUserAgents(queryIndex, true)}\"");
        }

        public async Task<HttpResponseMessage> BAPIGet(string requestUri)
        {
            try
            {
                return await client.GetAsync(requestUri);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.ExpectationFailed, ReasonPhrase = ex.Message };
            }
        }

        public async Task<HttpResponseMessage> BAPIPost(string requestUri, HttpContent content)
        {
            try
            {
                return await client.PostAsync(requestUri, content);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.ExpectationFailed, ReasonPhrase = ex.Message };
            }
        }
    }
}