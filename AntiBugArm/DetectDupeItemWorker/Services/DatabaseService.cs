using DetectDupeItemCore;
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

        public static Uri BaseAddress { get; set; }
        public static async Task<T> Execute<T>(QueryPayload data, Func<HttpResponseMessage, Task<T>> callback)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = BaseAddress;
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var res = await http.PostAsync("/query2json", content, Worker.ApplicationCancellationToken);
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
            return await Execute(data, async mes => await Task.FromResult(mes.IsSuccessStatusCode));
        }

        public static async Task<List<T>> Execute<T>(QueryPayload data) where T : class, new()
        {
            var resp = await Execute(data, async response =>
            {
                if (!response.IsSuccessStatusCode)
                {
                    _logger.Info($"Http response {data.Sql} [status={response.StatusCode}]");
                }
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<List<T>>(jsonContent);
                    return model;
                }
                _logger.Error($"Query server response status fail {response.StatusCode}: {JsonConvert.SerializeObject(data)}");
                return new List<T>();
            });
            return resp;
        }
    }
}
