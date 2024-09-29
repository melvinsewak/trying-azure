using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace learningapp.azure.function
{
    public class GetCourses
    {
        private readonly ILogger<GetCourses> _logger;
        private IConfiguration _configuration;
        public List<Course> Courses=new List<Course>();

        public GetCourses(ILogger<GetCourses> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [Function("GetCourses")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
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
                        Courses.Add(new Course() {CourseID=Int32.Parse(sqlDatareader["CourseID"].ToString()),
                        CourseName=sqlDatareader["CourseName"].ToString(),
                        Rating=Decimal.Parse(sqlDatareader["Rating"].ToString())});
                    }
            }

            return (IActionResult)Courses;
        }
    }
}
