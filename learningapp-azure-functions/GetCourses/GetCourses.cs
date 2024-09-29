using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using GetCourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function started processing a request.");
            List<Course> courses = new();
            courses.Add(new Course(){CourseID=1,CourseName="Dummy",Rating=5});
            var jsonStr = JsonConvert.SerializeObject(courses);
            _logger.LogInformation("C# HTTP trigger function finished processing a request.");
            return new OkObjectResult(jsonStr);
        }
    }
}
