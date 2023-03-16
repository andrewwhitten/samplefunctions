using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace andrewwhitten.samples
{
    public class LiableToTakeAWhileServiceResult
    {
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public string? Summary { get; set; }

        public int SecondsTaken { get; set; }
    }

    public class LiableToTakeAWhileService
    {
        private readonly ILogger _logger;

        public LiableToTakeAWhileService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LiableToTakeAWhileService>();
        }

        [Function("LiableToTakeAWhileService")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            HttpStatusCode code = HttpStatusCode.OK;
            bool success = true;
            string summary = String.Empty;
            int time = 50;

            var result = new LiableToTakeAWhileServiceResult
            {
                Date = DateTime.Now,
                Success = success,
                Summary = summary,
                SecondsTaken = time
            };

            HttpResponseData responseValue = req.CreateResponse(code);
            responseValue.Headers.Add("Content-Type", "application/json; charset=utf-8");
            responseValue.WriteString(JsonSerializer.Serialize(result));

            return responseValue;
        }
    }
}
