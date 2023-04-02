using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace andrewwhitten.samples
{
    /// <summary>
    /// Class <c>RandomlyUnavailableServiceResult</c> models a result of a serice call
    /// </summary>
    public class RandomlyUnavailableServiceResult
    {
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public string? Summary { get; set; }
    }

    /// <summary>
    /// Class <c>RandomlyUnavailableService</c> will sometimes succeed, and somtimes 503 fail
    /// </summary>
    public class RandomlyUnavailableService
    {
        private readonly ILogger _logger;

        public RandomlyUnavailableService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RandomlyUnavailableService>();
        }

        [Function("RandomlyUnavailableService")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            HttpStatusCode code = HttpStatusCode.OK;

            string summary = String.Empty;

            System.Random r = new Random();

            // This shoulf fail half the time....
            bool success = r.NextDouble() > 0.5;

            RandomlyUnavailableServiceResult result;

            if(success) {

                result = new RandomlyUnavailableServiceResult
                {
                    Date = DateTime.Now,
                    Success = success,
                    Summary = "A successful web service call"
                };

            } else {

                result = new RandomlyUnavailableServiceResult
                {
                    Date = DateTime.Now,
                    Success = success,
                    Summary = "An unsuccessful web service call. Maybe try again later?"
                };

                // Indicate Service Unavailable 503 code
                code = HttpStatusCode.ServiceUnavailable;

            }

            // Build the response
            HttpResponseData responseValue = req.CreateResponse(code);
            responseValue.Headers.Add("Content-Type", "application/json; charset=utf-8");
            responseValue.WriteString(JsonSerializer.Serialize(result));

            return responseValue;
        }
    }
}
