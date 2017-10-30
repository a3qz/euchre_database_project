#r "System.Configuration"
#r "System.Data"
#r "Newtonsoft.Json"

using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;
    
    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();
    
    string gameid = Guid.NewGuid().ToString();
    string team1score = data.team1score;
    string team2score = data.team2score;
    DateTime myDateTime = DateTime.Now;
    string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    string winner = (Int32.Parse(team1score) > Int32.Parse(team2score)) ? "1" : "2";

    using (SqlConnection conn = new SqlConnection(str))
    {
        conn.Open();
        var text = $"INSERT INTO Game VALUES('{gameid}', {winner}, {team1score}, {team2score}, '{sqlFormattedDate}');";

        using (SqlCommand cmd = new SqlCommand(text, conn))
        {
            // Execute the command and log the # rows affected.
            var rows = await cmd.ExecuteNonQueryAsync();
            log.Info($"{rows} rows were updated");
            return req.CreateResponse(HttpStatusCode.OK, "Hello " + rows);
            
        }
    }   
    return name == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
}
