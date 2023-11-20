using DetectDupeItem.Models;
using DetectDupeItem.Services;
using DetectDupeItemCore.Services;
using log4net;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DetectDupeItemWorker.Events
{

    public static class CayThong
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static Encoding _encoding;
        public static async Task Tracking(string fileName)
        {
            if (_encoding == null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _encoding = Encoding.GetEncoding("GB2312");
            }
            Console.WriteLine($"Begin tracking {fileName} at {DateTime.Now}");
            int lineNumbers = await GetLastLineNumber(fileName);
            var lineDatas = await GmLogService.GetLines(fileName, _encoding);
            var lines = lineDatas.Skip(lineNumbers).GetEnumerator();
            while (lines.MoveNext())
            {
                ++lineNumbers;
                UserInfo userInfo = null;
                bool isValid = false;
                try
                {
                    var line = lines.Current.Trim().Split("--", StringSplitOptions.RemoveEmptyEntries).Select(x=> x.Trim());
                    if (line.Count() != 2) continue;
                    var userId = line.FirstOrDefault();
                    var createdDate = GetCreatedDate(line.LastOrDefault());
                    userInfo = await GetUserInfo(userId);
                    isValid = await StateIsValid(userId, fileName, createdDate);
                    Console.WriteLine($"Tracking {fileName}={lines.Current.Trim()} line {lineNumbers}");
                    _logger.Info($"Tracking {fileName}={lines.Current.Trim()} line {lineNumbers}");
                    await AddToDb(userInfo, fileName, lineNumbers, isValid, createdDate);
                    if (!isValid)
                    {
                        _ = Task.Run(async () =>
                        {
                            await WinService.BlockIP(userInfo.IP);
                            await Task.Delay(60000);
                            await WinService.UnblockIP(userInfo.IP);
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error((object)ex.GetBaseException().Message);
                    _logger.Error((object)ex.GetBaseException().StackTrace);
                    continue;
                }
            }
        }
        private static DateTime GetCreatedDate(string input)
        {
            string text = input.Split(new string[1] { "--" }, StringSplitOptions.None).LastOrDefault();
            string format = "ddd MMM dd HH:mm:ss yyyy";
            if (DateTime.TryParseExact(text?.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            _logger.Error((object)("Cannot convert " + text + " to DateTime"));
            return DateTime.Now;
        }
        private static async Task<int> GetLastLineNumber(string fileName)
        {
            QueryPayload queryPayload = new QueryPayload();
            queryPayload.Sql = "SELECT * FROM web_dupe_item d where d.file_name = ? order by id desc";
            queryPayload.Payload = new string[1] { fileName };
            return (await DatabaseService.Execute<WebDupeItemEntity>(queryPayload)).FirstOrDefault()?.LineNumber ?? 0;
        }

        private static async Task<UserInfo> GetUserInfo(string userId)
        {
            QueryPayload queryPayload = new QueryPayload();
            queryPayload.Sql = "select u.ip, a.online as is_banned, u.id as user_id, u.name as player_name, u.account_id, a.name as username from cq_user as u inner join account a on a.id = u.account_id where u.id = ?";
            queryPayload.Payload = new string[1] { userId };
            return (await DatabaseService.Execute<UserInfo>(queryPayload)).FirstOrDefault();
        }
        private static async Task<bool> StateIsValid(string userId, string fileName, DateTime time)
        {
            var maxDate = time.AddMilliseconds(-time.Millisecond);
            var minDate = maxDate.AddSeconds(-3);
            var query = new QueryPayload()
            {
                Sql = "SELECT * FROM web_dupe_item WHERE (created_date BETWEEN ? AND ?) AND file_name = ? AND user_id = ?",
                Payload = new object[] { minDate, maxDate, fileName, userId }
            };
            var result = await DatabaseService.Execute<WebDupeItemEntity>(query);
            return !result.Any();
        }

        private static async Task<bool> AddToDb(UserInfo user, string fileName, int lineNumber, bool valid, DateTime createdDate)
        {
            if (user == null) return false;
            QueryPayload query = new QueryPayload
            {
                Sql = "INSERT INTO web_dupe_item(player_name, username, account_id, user_id, robot_id, primary_id, primary_type, second_id, second_type, created_date, line_number, file_name, is_valid, type) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                Payload = new object[14]
                {
                user?.PlayerName ?? string.Empty,
                user?.Username ?? string.Empty,
                user?.AccountId ?? 0,
                user?.UserId ?? 0,
                0,
                0,
                0,
                0,
                0,
                createdDate,
                lineNumber,
                fileName,
                valid ? '1' : '0',
                2
                }
            };
            bool num = await DatabaseService.ExecuteNonResult(query);
            if (!num)
            {
                _logger.Error((object)("Cannot insert to database -> " + JsonConvert.SerializeObject((object)query) + "]"));
            }
            return num;
        }
    }
}
