#r "System.Configuration"
#r "System.Data"
#r "Newtonsoft.Json"

using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// do this onc you find the gameid; then send this along.  Probably do from the server but maybe not.  
public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    var str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;
    dynamic test = await req.Content.ReadAsStringAsync();
    log.Info(test);
    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;
    
    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();
    

    string gameid = data.gameid;
    string partner1netid = data.partner1netid;
    string partner2netid = data.partner2netid;
    log.Info(partner2netid);
    int partner1score = Int32.Parse(data.partner1score.ToString());
    int partner2score = Int32.Parse(data.partner2score.ToString());
    int teamnumber = Int32.Parse(data.teamnumber.ToString());
    if (teamnumber >2 || teamnumber <1 || partner1score > 14 ||partner2score > 14){
        return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid values");
    }


    using (SqlConnection conn = new SqlConnection(str))
    {
        conn.Open();
        var text = $"UPDATE Partner SET partner1netid='{partner1netid}', partner2netid='{partner2netid}', partner1score={partner1score}, partner2score={partner2score}, teamnumber={teamnumber} WHERE gameid = '{gameid}';";

        using (SqlCommand cmd = new SqlCommand(text, conn))
        {
            // Execute the command and log the # rows affected.
            var rows = await cmd.ExecuteNonQueryAsync();
            log.Info($"{rows} rows were updated");
            return req.CreateResponse(HttpStatusCode.OK, "Hello " + rows);
            
        }
        
    }   
}
