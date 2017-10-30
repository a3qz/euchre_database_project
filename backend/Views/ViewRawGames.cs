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
    List<List<string>> final = new List<List<string>>();
    
    using (SqlConnection conn = new SqlConnection(str))
    {
        conn.Open();
        SqlCommand command = new SqlCommand("Select * from Partner;", conn);
        // int result = command.ExecuteNonQuery();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read()){
                List <string> temp = new List<string>();
                string gameid =(reader["gameid"].ToString());
                temp.Add(gameid);
                string partner1netid =(reader["partner1netid"].ToString());
                temp.Add(partner1netid);
                string partner2netid =(reader["partner2netid"].ToString());
                temp.Add(partner2netid);
                string partner1score =(reader["partner1score"].ToString());
                temp.Add(partner1score);
                string partner2score =(reader["partner2score"].ToString());
                temp.Add(partner2score);
                string teamnumber =(reader["teamnumber"].ToString());
                temp.Add(teamnumber);

                final.Add(temp);
            }
            return req.CreateResponse(HttpStatusCode.OK, final);
        }
    }   
}
