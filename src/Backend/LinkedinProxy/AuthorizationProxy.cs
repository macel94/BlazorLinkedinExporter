using System.Net;
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
        public HttpResponseData GetAccessToken([HttpTrigger(AuthorizationLevel.Anonymous, Http.Post)] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
