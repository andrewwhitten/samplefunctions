using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace andrewwhitten.samples
{
    public class WebServiceResult
    {
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public string? Summary { get; set; }

        public int Score { get; set; }
    }

    public class UnreliableEnterpriseService
    {
        private readonly ILogger _logger;

        public UnreliableEnterpriseService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UnreliableEnterpriseService>();
        }

        [Function("UnreliableEnterpriseService")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            System.Random r = new Random();

            int randValue = r.Next(0, 4);

            HttpStatusCode code = HttpStatusCode.OK;
            bool success = false;
            string summary = String.Empty;
            int score = -1;

            var result = new WebServiceResult
            {
                Date = DateTime.Now,
                Success = success,
                Summary = summary,
                Score = score
            };

            if(randValue == 0) {

                code = HttpStatusCode.OK;
                result.Success = true;
                result.Summary = "Operation succesful";
                result.Score = r.Next(1, 99);

            } else if(randValue == 1) {

                code = HttpStatusCode.BadRequest;
                result.Success = false;
                result.Summary = "Operation not succesful";

            } else if(randValue == 2) {

                code = HttpStatusCode.TooManyRequests;
                result.Success = false;
                result.Summary = "Operation too many request";
                
            }else if(randValue == 3) {

                code = HttpStatusCode.NotFound;
                result.Success = false;
                result.Summary = "Operation not found";

            } else if(randValue == 4) {

                code = HttpStatusCode.Forbidden;
                result.Success = false;
                result.Summary = "Operation forbidden";

            } 



            HttpResponseData responseValue = req.CreateResponse(code);
            responseValue.Headers.Add("Content-Type", "application/json; charset=utf-8");
            responseValue.WriteString(JsonSerializer.Serialize(result));

            return responseValue;
        }
    }
}
