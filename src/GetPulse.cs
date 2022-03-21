using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace src
{
    public class GetPulse
    {
        private readonly ILogger _logger;
        private static readonly HttpClient ghClient = new HttpClient();


        public GetPulse(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetPulse>();
        }

        [Function("GetPulse")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "getPulse/{onwer:alpha}/{project:alpha}")] HttpRequestData req, string onwer, string project)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            ghClient.DefaultRequestHeaders.Accept.Clear();
            ghClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            ghClient.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");


            var stringTask = ghClient.GetStreamAsync($"https://api.github.com/repos/{onwer}/{project}/stats/participation");

            var participations = JsonSerializer.Deserialize<Participations>(await stringTask);
            _logger.LogInformation("--- participations");

            var response = req.CreateResponse();
            response.Headers.Add("Content-Type", "application/json");

            _logger.LogInformation("--- Last call");
            await response.WriteAsJsonAsync<Participations>(participations,HttpStatusCode.OK) ;
            
            _logger.LogInformation("--- returning");
            return response;

        }
    }

    public class Participations
    {
        public List<int> all { get; set; }
        public List<int> owner { get; set; }
    }
}
