using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using GetCourses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace learningapp.azure.function
{
    public class GetCourses
    {
        private readonly ILogger<GetCourses> _logger;
        private readonly IConfiguration _configuration;

        public GetCourses(ILogger<GetCourses> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [Function("GetCourses")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function started processing a request.");
            List<Course> courses = new();
            //courses.Add(new Course(){CourseID=1,CourseName="Dummy",Rating=5});
            
            var commonSettings = _configuration.GetSection("Common:Settings");
            var dbPassword = commonSettings.GetValue<string>("dbPassword");
            string connectionString = string.Format(commonSettings.GetValue<string>("AZURE_SQL_CONNECTIONSTRING")!, dbPassword);

            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            var sqlcommand = new SqlCommand(
            "SELECT CourseID,CourseName,Rating FROM Course;",sqlConnection);
            using (SqlDataReader sqlDatareader = sqlcommand.ExecuteReader())
            {
                while (sqlDatareader.Read())
                    {
                        courses.Add(new Course() {CourseID=Int32.Parse(sqlDatareader["CourseID"].ToString()),
                        CourseName=sqlDatareader["CourseName"].ToString(),
                        Rating=Decimal.Parse(sqlDatareader["Rating"].ToString())});
                    }
            }
            
            var jsonStr = JsonConvert.SerializeObject(courses);
            _logger.LogInformation("C# HTTP trigger function finished processing a request.");
            return new OkObjectResult(jsonStr);
        }
    }
}
