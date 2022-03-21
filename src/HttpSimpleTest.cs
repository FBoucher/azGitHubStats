using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace C5M.Function
{
    public class HttpSimpleTest
    {
        private readonly ILogger _logger;

        public HttpSimpleTest(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpSimpleTest>();
        }

        [Function("HttpSimpleTest")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "httpsimpletest/{onwer:alpha}/{project:alpha}")] HttpRequestData req, string onwer, string project, FunctionContext executionContext)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions! " + onwer + "..." + project);

            return response;
        }
    }
}

