using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace andrewwhitten.samples
{

    public class BadlyWrittenCalculatorService
    {
        private readonly ILogger _logger;

        public BadlyWrittenCalculatorService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BadlyWrittenCalculatorService>();
        }

        [Function("BadlyWrittenCalculatorAddService")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var param1 = query["param1"];
            var param2 = query["param2"];

            int p1, p2 = 0;
            
            bool parseSuccess1 = Int32.TryParse(param1, out p1);
            bool parseSuccess2 = Int32.TryParse(param2, out p2);

            HttpStatusCode code = HttpStatusCode.OK;
            BadlyWrittenCalculatorServiceResult result;

            if(!parseSuccess1 || !parseSuccess2) {

                code = HttpStatusCode.BadRequest;
                bool success = false;
                string summary = "request parameters not valid";
                int score = -1;

                result = new BadlyWrittenCalculatorServiceResult
                {
                    Date = DateTime.Now,
                    Success = success,
                    Summary = summary,
                    Result = score
                };
            }
            else {

                bool success = true;
                string summary = String.Empty;
                int score = p1+p2;

                result = new BadlyWrittenCalculatorServiceResult
                {
                    Date = DateTime.Now,
                    Success = success,
                    Summary = summary,
                    Result = score
                };
            }

            HttpResponseData responseValue = req.CreateResponse(code);
            responseValue.Headers.Add("Content-Type", "application/json; charset=utf-8");
            responseValue.WriteString(JsonSerializer.Serialize(result));

            return responseValue;
        }
    }
}
