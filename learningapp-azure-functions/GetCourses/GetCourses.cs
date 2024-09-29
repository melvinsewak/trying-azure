using System.Text.Json.Serialization;
using GetCourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace learningapp.azure.function
{
    public class GetCourses
    {
        private readonly ILogger<GetCourses> _logger;

        public GetCourses(ILogger<GetCourses> logger)
        {
            _logger = logger;
        }

        [Function("GetCourses")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(JsonConvert.SerializeObject(new Course(){CourseID=1,CourseName="Dummy",Rating=5}));
        }
    }
}
