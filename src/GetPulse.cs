using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Octokit;

namespace src
{
    public class GetPulse
    {
        private readonly ILogger _logger;

        public GetPulse(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetPulse>();
        }

        [Function("GetPulse")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var ghClient = new GitHubClient(new ProductHeaderValue("powertoys"));

            var recently = new RepositoryIssueRequest{
                    Filter = IssueFilter.All,
                    State = ItemStateFilter.All,
                    Since = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7))
                };
            var issues = ghClient.Issue.GetAllForRepository("microsoft", "powertoys", recently).Result;
            

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            //response.Headers.Add("Content-Type", "application/json");
            //response.Body.Write(issues);
            
            return response;
        }
    }
}
