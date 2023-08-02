using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;

namespace Yandex.API
{
    /// <summary>
    /// Routing api client
    /// </summary>
    /// <see cref="https://yandex.ru/dev/router/doc/ru/request"/>
    public class RoutingClient
    {
        private readonly string apiKey;
        private readonly HttpClient client;

        private static HttpClient DefaultClient = new HttpClient();

        public RoutingClient(string apiKey)
        {
            this.apiKey = apiKey;
            client = DefaultClient;
        }

        public RoutingClient(HttpClient client, string apiKey)
        {
            if (client == null) throw new ArgumentNullException("client");
            this.client = client;
        }

        private static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public async Task<RoutingResponse> Route(RoutingRequest request,
           CancellationToken cancellationToken = default)
        {
            var collection = new System.Collections.Specialized.NameValueCollection
            {
                ["apikey"] = apiKey
            };
            request.Fill(collection);
            var isFirst = true;
            var resultUrl = "https://api.routing.yandex.net/v2/route?";
            foreach (var key in collection.AllKeys)
            {
                if (!isFirst)
                    resultUrl += "&";
                resultUrl += key + "=" + HttpUtility.UrlEncode(collection[key]);
                isFirst = false;
            }
            var response = await client.GetAsync(resultUrl, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RoutingResponse>(result, Settings);
        }
    }
}
