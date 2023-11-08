using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DetectDupeItem.Services
{
    internal class DatabaseService
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string PathQuery => Environment.GetCommandLineArgs()[2] + "/query2json";
        public static async Task<T> Execute<T>(QueryPayload data, Func<HttpResponseMessage, Task<T>> callback)
        {
            using (var http = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var res = await http.PostAsync(PathQuery, content);
                if (res.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var contentErr = await res.Content.ReadAsStringAsync();
                    _logger.Error($"Query error {contentErr}");
                }
                return await callback(res);
            }
        }

        public static async Task<bool> ExecuteNonResult(QueryPayload data)
        {
            return await Execute(data, async mes => mes.IsSuccessStatusCode);
        }

        public static async Task<List<T>> Execute<T>(QueryPayload data) where T : class, new()
        {
            var resp = await Execute(data, async mes =>
            {
                _logger.Info($"Http response {data.Sql} [status={mes.StatusCode}]");
                if (mes.IsSuccessStatusCode)
                {
                    var jsonContent = await mes.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<List<T>>(jsonContent);
                    return model;
                }
                _logger.Error($"Query server response status fail {mes.StatusCode}: {JsonConvert.SerializeObject(data)}");
                return new List<T>();
            });
            return resp;
        }
    }
}
