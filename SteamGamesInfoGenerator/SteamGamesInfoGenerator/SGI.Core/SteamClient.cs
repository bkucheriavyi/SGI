using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApplication2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SGI.Core
{
    public class SteamClient
    {
        public SteamGames GetCurrentApplicationList()
        {
            var response = HttpRequest("http://api.steampowered.com/ISteamApps/GetAppList/v2");

            return response != string.Empty ? JsonConvert.DeserializeObject<SteamGames>(response) : new SteamGames();
        }

        public async Task<PriceOverview> GetPriceOverview(string appId, string currency)
        {
            var result = new PriceOverview();
            try
            {
                var requestUri = string.Format("http://store.steampowered.com/api/appdetails/?appids={0}&cc={1}&v=1",
                    appId, currency);
                var response = await GetJsonAsync(requestUri);
                var parsedJsonRequest = JObject.Parse(response);
                if (parsedJsonRequest[appId]["success"].ToString().ToLower() == "false")
                    return new PriceOverview();
                return JsonConvert.DeserializeObject<PriceOverview>(parsedJsonRequest[appId]["data"]["price_overview"].ToString());
            }
            catch (Exception ex)
            {

            }

            return result;
        }
  
        private string HttpRequest(string requestUri)
        {
            var request = (HttpWebRequest) WebRequest.Create(@requestUri);
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                if (responseStream == null) return string.Empty;

                using (var reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public IEnumerable<PriceOverview> GetPriceOverviewAsync(IEnumerable<string> appIds)
        {
            var tasks= appIds.Select(async appId => await GetPriceOverview(appId, "ru"));
            return Task.WhenAll(tasks).Result;
        }
        public static async Task<string> GetJsonAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(uri).ConfigureAwait(false);
            }
        }
    }
}
