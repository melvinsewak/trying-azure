using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace learningapp.Pages;

public class IndexModel : PageModel
{
    public string version { get; set; }
     public List<Course> Courses=new List<Course>();
    private readonly ILogger<IndexModel> _logger;
    private IConfiguration _configuration;
    public IndexModel(ILogger<IndexModel> logger,IConfiguration configuration)
    {
        _logger = logger;
        _configuration=configuration;
    }

    public async Task<IActionResult> OnGet()
    {
        // var commonSettings = _configuration.GetSection("Common:Settings");
        // version = commonSettings.GetValue<string>("version")!;
        // var dbPassword = commonSettings.GetValue<string>("dbPassword");
        // string connectionString = string.Format(commonSettings.GetValue<string>("AZURE_SQL_CONNECTIONSTRING")!, dbPassword);

        // var sqlConnection = new SqlConnection(connectionString);
        // sqlConnection.Open();

        // var sqlcommand = new SqlCommand(
        // "SELECT CourseID,CourseName,Rating FROM Course;",sqlConnection);
        //  using (SqlDataReader sqlDatareader = sqlcommand.ExecuteReader())
        //  {
        //      while (sqlDatareader.Read())
        //         {
        //             Courses.Add(new Course() {CourseID=Int32.Parse(sqlDatareader["CourseID"].ToString()),
        //             CourseName=sqlDatareader["CourseName"].ToString(),
        //             Rating=Decimal.Parse(sqlDatareader["Rating"].ToString())});
        //         }
        //  }

        string functionURL = "https://learningapp-azfuncapp.azurewebsites.net/api/GetCourses";

        using(HttpClient client = new HttpClient()){
            HttpResponseMessage msg = await client.GetAsync(functionURL);
            string content = await msg.Content.ReadAsStringAsync();
            Courses = JsonConvert.DeserializeObject<List<Course>>(content)!;
        }

        return Page();
    }
}
