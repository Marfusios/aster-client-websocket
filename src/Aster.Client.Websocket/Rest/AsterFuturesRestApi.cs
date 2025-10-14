using System.Net.Http;
using Aster.Client.Websocket.Signing;

namespace Aster.Client.Websocket.Rest
{
    public abstract class AsterFuturesRestApi : AsterRestApi
    {
        protected const string DEFAULT_FUTURES_BASE_URL = "https://fapi.asterdex.com";

        public AsterFuturesRestApi(HttpClient httpClient, string? apiKey, string? apiSecret, string baseUrl = DEFAULT_FUTURES_BASE_URL)
            : base(httpClient, baseUrl: baseUrl, apiKey: apiKey, apiSecret: apiSecret)
        {
        }

        public AsterFuturesRestApi(HttpClient httpClient, string? apiKey, IAsterSignatureService signatureService, string baseUrl = DEFAULT_FUTURES_BASE_URL)
            : base(httpClient, baseUrl: baseUrl, apiKey: apiKey, signatureService: signatureService)
        {
        }
    }
}
