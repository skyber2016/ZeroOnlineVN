using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace DetectDupeItem
{
    internal class cq_action
    {
        public string id { get; set; }
        public string id_next { get; set; }
        public string id_nextfail { get; set; }
        public string type { get; set; }
        public string data { get; set; }
        public string param { get; set; }
        public static cq_action FromSqlInsert(string sqlInsert)
        {
            // Loại bỏ các ký tự không mong muốn từ câu lệnh SQL
            sqlInsert = sqlInsert.Replace("INSERT INTO `cq_action_copy1` VALUES (", "").TrimEnd(')', '\r', ';');

            // Tách các giá trị bằng dấu phẩy
            string[] values = sqlInsert.Split(',').Select(x => x.Replace("'", string.Empty).Trim()).ToArray();

            // Tạo một đối tượng cq_action từ giá trị của câu lệnh INSERT
            var cqAction = new cq_action
            {
                id = values[0].Trim(),
                id_next = values[1].Trim(),
                id_nextfail = values[2].Trim(),
                type = values[3],
                data = values[4],
                param = values[5].Trim('\'').Trim()
            };

            return cqAction;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var list = new StringBuilder();
            var allow = new string[] { "0101", "0102", "1010", "0125", "0126", "0130" };
            foreach (var item in GetDatas())
            {
                if (allow.Contains(item.type))
                {
                    //item.param = StringHelper.RemoveDiacritics(item.param);
                    list.AppendLine($"UPDATE cq_action SET param = '{item.param}' WHERE id = {item.id};");
                }

            }
            File.WriteAllText("cq_action_dec.sql", list.ToString());

        }

        static string GetUnicodeString(string input)
        {
            string unicodeString = "";
            foreach (char c in input)
            {
                unicodeString += "\\u" + ((int)c).ToString("X4");
            }
            return unicodeString;
        }

        static IEnumerable<cq_action> GetDatas()
        {
            var source = File.ReadAllText(@"C:\Users\duynh2\Downloads\cq_action_copy1.sql", Encoding.UTF8);
            var gb2312 = Encoding.GetEncoding("gb2312");
            var str = source;
            var lines = str.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                yield return cq_action.FromSqlInsert(lines[i]);
            }
        }

        static List<cq_action> Update(cq_action item)
        {
            using (var http = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new
                {
                    sql = "UPDATE cq_action set param = ? where id = ?",
                    payload = new object[] { item.param, item.id }
                });
                var request = new StringContent(json, Encoding.UTF8, "application/json");
                Console.Write($"{item.id}=");
                var response = http.PostAsync("http://103.188.166.96:3001/query2json", request);
                response.Wait();
                Console.WriteLine(response.Result.StatusCode);
                var content = response.Result.Content.ReadAsStringAsync();
                content.Wait();
                return JsonConvert.DeserializeObject<List<cq_action>>(content.Result);
            }
        }


    }
}
