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
    
    // parse string
    string jsonContent = await req.Content.ReadAsStringAsync();
    JToken activityLog = JObject.Parse(jsonContent.ToString());
    string name = (activityLog.SelectToken("name").ToString());
    string netid = (activityLog.SelectToken("netid").ToString()); 



    using (SqlConnection conn = new SqlConnection(str))
    {
        conn.Open();
        var text = $"INSERT INTO Player VALUES('{netid}', 0, 0, '{name}');";

        using (SqlCommand cmd = new SqlCommand(text, conn))
        {
            // Execute the command and log the # rows affected.
            var rows = await cmd.ExecuteNonQueryAsync();
            log.Info($"{rows} rows were updated");
            return req.CreateResponse(HttpStatusCode.OK, "Hello " + rows);
            
        }
    }   
}
