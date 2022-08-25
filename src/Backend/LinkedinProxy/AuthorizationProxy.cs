using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Google.Protobuf.WellKnownTypes;
using LinkedinProxy.Dto.Authentication;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace LinkedinProxy
{
    public class AuthorizationProxy
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthorizationProxy(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AuthorizationProxy>();
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [Function($"accessToken")]
        public async Task<HttpResponseData> GetAccessTokenAsync([HttpTrigger(AuthorizationLevel.Anonymous, Http.Post)] HttpRequestData req)
        {
            FormUrlEncodedContent content = await GetRequestContentAsync(req);

            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("https://www.linkedin.com/oauth/v2/accessToken"),
                Content = content,
                Method = HttpMethod.Post
            };

            var postResult = await client.SendAsync(httpRequestMessage);
            var response = req.CreateResponse(postResult.StatusCode);

            if (postResult.IsSuccessStatusCode)
            {
                var result = await postResult.Content.ReadFromJsonAsync<TokenResponse?>();
                await response.WriteAsJsonAsync(result);
            }
            else
            {
                await response.WriteStringAsync(await postResult.Content.ReadAsStringAsync());
            }

            return response;
        }

        private async Task<FormUrlEncodedContent> GetRequestContentAsync(HttpRequestData req)
        {
            var requestBody = string.Empty;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var entries = requestBody.Split("&");

            var data = new List<KeyValuePair<string, string>>();
            foreach (var entry in entries)
            {
                var splitEntry = entry.Split("=");
                var param = splitEntry[0];
                var value = param.Equals("redirect_uri") ? Uri.UnescapeDataString(splitEntry[1]) : splitEntry[1];
                data.Add(new KeyValuePair<string, string>(param, value));
            }

            data.Add(new KeyValuePair<string, string>("client_secret", _configuration["LinkedinClientSecret"]));

            var content = new FormUrlEncodedContent(data);
            return content;
        }

        [Function($"emailAddress")]
        public async Task<HttpResponseData> GetEmailAsync([HttpTrigger(AuthorizationLevel.Anonymous, Http.Get)] HttpRequestData req)
        {
            var authHeaderValues = req.Headers.FirstOrDefault(x => x.Key.Equals("Authorization")).Value ?? new List<string>();
            var authHeaderValue = string.Join(" ", authHeaderValues);
            var authenticationHeaderValue = AuthenticationHeaderValue.TryParse(authHeaderValue, out var parsedAuthHeader);

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = parsedAuthHeader;

            var res = await client.GetFromJsonAsync<GetEmailResponse?>("https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(res);
            return response;
        }
    }
}
