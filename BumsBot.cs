using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BumsBot
{

    public class BumsBots
    {

        public readonly BumsBotQuery PubQuery;
        private readonly ProxyType[] PubProxy;
        private readonly string AccessToken;
        public readonly bool HasError;
        public readonly string ErrorMessage;
        public readonly string IPAddress;

        public BumsBots(BumsBotQuery Query, ProxyType[] Proxy)
        {
            PubQuery = Query;
            PubQuery.Auth = GetQuery();
            PubProxy = Proxy;
            IPAddress = GetIP().Result;
            var Login = BumsLogin().Result;
            if (Login is not null)
            {
                AccessToken = Login.Data.Token;
                HasError = false;
                ErrorMessage = "";
            }
            else
            {
                AccessToken = string.Empty;
                HasError = true;
                ErrorMessage = "login failed";
            }
        }

        private string GetQuery()
        {
            string query;
            TelegramMiniApp.WebView vw = new(PubQuery.API_ID, PubQuery.API_HASH, PubQuery.Name, PubQuery.Phone, "bums_ton_bot", "https://app.bums.bot/");
            vw.Get_tgWebAppData(out query);

            return query;
        }

        private async Task<string> GetIP()
        {
            HttpClient client;
            var FProxy = PubProxy.Where(x => x.Index == PubQuery.Index);
            if (FProxy.Count() != 0)
            {
                if (!string.IsNullOrEmpty(FProxy.ElementAtOrDefault(0)?.Proxy))
                {
                    var handler = new HttpClientHandler() { Proxy = new WebProxy() { Address = new Uri(FProxy.ElementAtOrDefault(0)?.Proxy ?? string.Empty) } };
                    client = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, 30) };
                }
                else
                {
                    client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
                }
            }
            else
            {
                client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            }
            HttpResponseMessage httpResponse = null;
            try
            {
                httpResponse = await client.GetAsync($"https://httpbin.org/ip");
            }
            catch { }
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<Httpbin>(responseStream);
                    return responseJson?.Origin ?? string.Empty;
                }
            }

            return "";
        }

        private async Task<BumsLoginResponse?> BumsLogin()
        {
            var BAPI = new BumsApi(0, PubQuery.Auth, PubQuery.Index, PubProxy);
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("invitationCode", ""), new KeyValuePair<string, string>("initData", PubQuery.Auth) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/user/telegram_auth", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsLoginResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<BumsGameInfoResponse?> BumsGameInfo()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://api.bums.bot/miniapps/api/user_game_level/getGameInfo");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions() { NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString };
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsGameInfoResponse>(responseStream, options);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BumsDailyReward()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/sign/sign", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return (responseJson?.Code == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<bool> BumsCollectCoin(int collectSeqNo, int collectAmount)
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            string hashCode = Tools.getMD5Hash(collectAmount.ToString() + collectSeqNo + "7be2a16a82054ee58398c5edb7ac4a5a");
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("hashCode", hashCode), new KeyValuePair<string, string>("collectSeqNo", collectSeqNo.ToString()), new KeyValuePair<string, string>("collectAmount", collectAmount.ToString()) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/user_game/collectCoin", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return (responseJson?.Code == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<List<BumsAnswersResponse>?> BumsAnswers()
        {
            var client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true, NoStore = true, MaxAge = TimeSpan.FromSeconds(0d) };
            HttpResponseMessage httpResponse = null;
            try
            {
                httpResponse = await client.GetAsync($"https://raw.githubusercontent.com/glad-tidings/BumsBot/refs/heads/main/tasks.json");
            }
            catch { }
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<List<BumsAnswersResponse>>(responseStream);
                    return responseJson;
                }
            }
            
            return null;
        }

        public async Task<BumsTaskResponse?> BumsTasks()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://api.bums.bot/miniapps/api/task/lists");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsTaskResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BumsFinishTask(int id, string pwd)
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            FormUrlEncodedContent formContent;
            if (string.IsNullOrEmpty(pwd))
                formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("id", id.ToString()) });
            else
                formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("id", id.ToString()), new KeyValuePair<string, string>("pwd", pwd) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/task/finish_task", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return (responseJson?.Code == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<BumsMineResponse?> BumsMines()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/mine/getMineLists", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions() { NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString };
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsMineResponse>(responseStream, options);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BumsUpgradeMine(int mineId)
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("mineId", mineId.ToString()) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/mine/upgrade", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return (responseJson?.Code == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<BumsSpinResponse?> BumsSpins()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://api.bums.bot/miniapps/api/mine/getMineLists");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsSpinResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BumsStartSpin(int propId)
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("propId", propId.ToString()) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/game_spin/Start", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return (responseJson?.Code == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<BumsLotteryAnswerResponse?> BumsLotteryAnswer()
        {
            var client = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true, NoStore = true, MaxAge = TimeSpan.FromSeconds(0d) };
            HttpResponseMessage httpResponse = null;
            try
            {
                httpResponse = await client.GetAsync($"https://raw.githubusercontent.com/glad-tidings/BumsBot/refs/heads/main/lottery.json");
            }
            catch { }
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsLotteryAnswerResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<BumsLotteryResponse?> BumsLottery()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://api.bums.bot/miniapps/api/mine_active/getMineAcctiveInfo");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsLotteryResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BumsJoinLottery(string cardIdStr)
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("cardIdStr", cardIdStr) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/mine_active/JoinMineAcctive", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsJoinLotteryResponse>(responseStream);
                    return (responseJson?.Data.Status == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<BumsBalanceResponse?> BumsBalance()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet($"https://api.bums.bot/miniapps/api/wallet/balance");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsBalanceResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BumsW70001To80001()
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/wallet/W70001To80001", null);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return (responseJson?.Code == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<bool> BumsUpgradeLevel(string @type)
        {
            var BAPI = new BumsApi(1, AccessToken, PubQuery.Index, PubProxy);
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("type", @type) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/user_game_level/upgradeLeve", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return (responseJson?.Code == 0 ? true : false);
                }
            }

            return false;
        }

        public async Task<BumsGangResponse?> BumsGangs()
        {
            var BAPI = new BumsApi(1, AccessToken, (int)PubQuery.Index, PubProxy);
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("boostNum", 15.ToString()), new KeyValuePair<string, string>("powerNum", 35.ToString()) });
            var httpResponse = await BAPI.BAPIPost($"https://api.bums.bot/miniapps/api/gang/gang_lists", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsGangResponse>(responseStream);
                    return responseJson;
                }
            }

            return null;
        }

        public async Task<bool> BumsJoinGang()
        {
            var BAPI = new BumsApi(1, AccessToken, (int)PubQuery.Index, PubProxy);
            var formContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("name", "gtbums") });
            var httpResponse = await BAPI.BAPIPost("https://api.bums.bot/miniapps/api/gang/gang_join", formContent);
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return responseJson?.Code == 0;
                }
            }

            return false;
        }

        public async Task<bool> BumsLeaveGang()
        {
            var BAPI = new BumsApi(1, AccessToken, (int)PubQuery.Index, PubProxy);
            var httpResponse = await BAPI.BAPIGet("https://api.bums.bot/miniapps/api/gang/gang_leave");
            if (httpResponse is not null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                    var responseJson = await JsonSerializer.DeserializeAsync<BumsPubResponse>(responseStream);
                    return responseJson?.Code == 0;
                }
            }

            return false;
        }

    }
}