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
    
    string tournamentid = Guid.NewGuid().ToString();
    string gameid = data.gameid;
    int roundnumber = Int32.Parse(data.roundnumber);

    using (SqlConnection conn = new SqlConnection(str))
    {
        conn.Open();
        var text = $"INSERT INTO Tournament VALUES('{tournamentid}', '{gameid}', {roundnumber});";

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
