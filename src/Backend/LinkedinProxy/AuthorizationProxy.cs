using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LinkedinProxy.Dto.Authentication;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace LinkedinProxy
{
    public class AuthorizationProxy
    {
        private readonly ILogger _logger;

        public AuthorizationProxy(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AuthorizationProxy>();
        }

        [Function($"accessToken")]
        public async Task<HttpResponseData> GetAccessToken([HttpTrigger(AuthorizationLevel.Anonymous, Http.Post)] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = string.Empty;
            using (StreamReader streamReader = new(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var entries = requestBody.Split("&");

            var data = new List<KeyValuePair<string, string>>();
            foreach(var entry in entries)
            {
                var splitEntry = entry.Split("=");
                var param = splitEntry[0];
                var value = param.Equals("redirect_uri") ? Uri.UnescapeDataString(splitEntry[1]) : splitEntry[1];
                data.Add(new KeyValuePair<string, string>(param, value));
            }

            var client = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("https://www.linkedin.com/oauth/v2/accessToken"),
                Content = new FormUrlEncodedContent(data),
                Method = HttpMethod.Post
            };

            var postResult = await client.SendAsync(httpRequestMessage);
            var result = await postResult.Content.ReadFromJsonAsync<TokenResponse?>();

            var response = req.CreateResponse(postResult.StatusCode);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
