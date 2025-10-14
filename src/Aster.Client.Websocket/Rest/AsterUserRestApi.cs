using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Aster.Client.Websocket.Rest.Responses;
using Aster.Client.Websocket.Signing;

namespace Aster.Client.Websocket.Rest
{
    public class AsterUserRestApi : AsterFuturesRestApi
    {
        private const string LISTEN_KEY_ENDPOINT = "/fapi/v1/listenKey";

        public AsterUserRestApi(string baseUrl = DEFAULT_FUTURES_BASE_URL, string? apiKey = null, string? apiSecret = null)
            : this(new HttpClient(), baseUrl: baseUrl, apiKey: apiKey, apiSecret: apiSecret)
        {
        }

        public AsterUserRestApi(HttpClient httpClient, string baseUrl = DEFAULT_FUTURES_BASE_URL, string? apiKey = null, string? apiSecret = null)
            : base(httpClient, baseUrl: baseUrl, apiKey: apiKey, apiSecret: apiSecret)
        {
        }

        public AsterUserRestApi(HttpClient httpClient, IAsterSignatureService signatureService, string baseUrl = DEFAULT_FUTURES_BASE_URL, string? apiKey = null)
            : base(httpClient, baseUrl: baseUrl, apiKey: apiKey, signatureService: signatureService)
        {
        }

        public Task<AsterListenKeyResponse> CreateListenKey()
        {
            return SendPublicAsync<AsterListenKeyResponse>(
                LISTEN_KEY_ENDPOINT,
                HttpMethod.Post);
        }

        public Task KeepAliveListenKey(string listenKey)
        {
            return SendPublicAsync<string>(
                LISTEN_KEY_ENDPOINT,
                HttpMethod.Put,
                query: new Dictionary<string, object>
                {
                    { "listenKey", listenKey },
                });
        }

        public Task CloseListenKey(string listenKey)
        {
            return SendPublicAsync<string>(
                LISTEN_KEY_ENDPOINT,
                HttpMethod.Delete,
                query: new Dictionary<string, object>
                {
                    { "listenKey", listenKey },
                });
        }
    }
}
