using API.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SqlKata.Execution
{
    public class ExecuteQuery
    {
        [JsonProperty("fieldCount")]
        public int FieldCount { get; set; }

        [JsonProperty("affectedRows")]
        public int AffectedRows { get; set; }

        [JsonProperty("insertId")]
        public int InsertId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
    public class QueryError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("errno")]
        public int ErrNo { get; set; }

        [JsonProperty("sqlMessage")]
        public string SqlMessage { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("sql")]
        public string Sql { get; set; }
    }
    public static class QueryExtension
    {
        public static async Task<T> Call<T>(this SqlResult query)
        {
            var loggingMessage = string.Empty;
            try
            {
                var dbAPI = QueryFactory.DbAPI;
                using (var http = new HttpClient())
                {
                    var payload = JsonConvert.SerializeObject(new
                    {
                        sql = query.Sql,
                        payload = query.Bindings.Select(x =>
                        {
                            try
                            {
                                if (x == null)
                                {
                                    return null;
                                }
                                if ((bool)x == false)
                                {
                                    x = 0;
                                }
                                else if ((bool)x == true)
                                {
                                    x = 1;
                                }
                            }
                            catch (Exception)
                            {

                            }
                            return x;
                        })
                    });
                    loggingMessage = $"Start call {dbAPI} with body {payload}";
                    var body = new StringContent(payload, Encoding.UTF8, "application/json");
                    var resp = await http.PostAsync(dbAPI, body);
                    loggingMessage = $"{dbAPI} response status code {resp.StatusCode}";
                    if (resp.IsSuccessStatusCode)
                    {
                        var jsonString = await resp.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(jsonString,
                        new JsonSerializerSettings()
                        {
                            ContractResolver = new MyContractResolver()
                        });
                    }
                    else
                    {
                        var message = await resp.Content.ReadAsStringAsync();
                        loggingMessage = $"{dbAPI} response error {message}";
                        return default;
                    }
                }
            }
            finally
            {
                QueryFactory.HttpLoggerError(loggingMessage);
            }
            
        }
    }
    public static class TaskExtension
    {
        public static T WaitAsync<T>(this Task<T> t)
        {
            t.Wait();
            return t.Result;
        }
        public static void WaitAsync(this Task t)
        {
            t.Wait();
        }
    }
}
