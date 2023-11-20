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

internal class CoreMerged
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
        var lines = (await GmLogService.GetLines(fileName, _encoding)).Skip(lineNumbers).GetEnumerator();
        while (lines.MoveNext())
        {
            if (!lines.Current.Trim().StartsWith("--"))
            {
                Console.WriteLine(lines.Current);
                _logger.Info($"Tracking {lines.Current}");
            }

            ++lineNumbers;
            string current = lines.Current;
            if (!current.StartsWith("主法宝的"))
            {
                continue;
            }
            lines.MoveNext();
            ++lineNumbers;
            string current2 = lines.Current;
            if (!current2.StartsWith("辅助法宝的"))
            {
                continue;
            }
            lines.MoveNext();
            ++lineNumbers;
            if (!lines.Current.StartsWith("合成后主法宝的[玩家名字"))
            {
                continue;
            }
            UserInfo user = null;
            Item item = null;
            try
            {
                item = GetItems(current, current2);
                user = await GetUserInfo(item);
                if (user == null)
                {
                    _logger.Error((object)("Could not be found user " + item.RobotId));
                    continue;
                }
                item.IsValid = await ItemValid(item, user, fileName, lineNumbers);
                if (item.IsValid)
                {
                    continue;
                }
                string notification = GetNotification(item, user, fileName, lineNumbers);
                _logger.Info((object)notification);
                await SendNotification(notification);
                if (await user.DoBannedAccount())
                {
                    _logger.Info((object)("Account " + user.Username + " has been banned"));
                }
                else
                {
                    _logger.Info((object)("Account " + user.Username + " blocking with error"));
                }
                continue;
            }
            catch (Exception ex)
            {
                _logger.Error((object)ex.GetBaseException().Message);
                _logger.Error((object)ex.GetBaseException().StackTrace);
                continue;
            }
            finally
            {
                await AddToDb(item, user, fileName, lineNumbers);
            }


        }
    }

    private static string GetNotification(Item item, UserInfo user, string fileName, int lineNumber)
    {
        int totalWidth = 20;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("--------------- CẢNH BÁO GIAN LẬN ---------------");
        stringBuilder.Append("**Loại:**".PadRight(totalWidth)).AppendLine("Hợp nhất lõi");
        stringBuilder.Append("**Tên đăng nhập:**".PadRight(totalWidth)).AppendLine(user?.Username ?? "Không xác định");
        stringBuilder.Append("**Tên nhân vật:**".PadRight(totalWidth)).AppendLine(user?.PlayerName.ToString() ?? "Không xác định");
        stringBuilder.Append("**Account_ID:**".PadRight(totalWidth)).AppendLine(user?.AccountId.ToString() ?? "Không xác định");
        stringBuilder.Append("**User_ID:**".PadRight(totalWidth)).AppendLine(user?.UserId.ToString() ?? "Không xác định");
        stringBuilder.Append("**Robot_ID:**".PadRight(totalWidth)).AppendLine(item.RobotId.ToString() ?? "Không xác định");
        stringBuilder.Append("**Item chính:**".PadRight(totalWidth)).AppendLine($"[ID={item.PrimaryId} TYPE={item.PrimaryTypeId}]");
        stringBuilder.Append("**Item phụ:**".PadRight(totalWidth)).AppendLine($"[ID={item.SecondId} TYPE={item.SecondTypeId}]");
        stringBuilder.Append("**Thời gian:**".PadRight(totalWidth)).AppendLine(item.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") ?? "Không xác định");
        stringBuilder.Append("**Tệp tin:**".PadRight(totalWidth)).AppendLine($"[FILE={fileName} LINE={lineNumber}]" ?? "Không xác định");
        return stringBuilder.ToString();
    }

    private static async Task<bool> SendNotification(string message)
    {
        BotMessageEntity botMessageEntity = new BotMessageEntity
        {
            Channel = "1125312849657483275",
            CreatedDate = DateTime.Now,
            IsSent = false,
            Message = Base64Encode(message)
        };
        QueryPayload queryPayload = new QueryPayload();
        queryPayload.Sql = "INSERT INTO bot_message(message, created_date, is_sent, channel) VALUES(?,?,?,?)";
        queryPayload.Payload = new object[4] { botMessageEntity.Message, botMessageEntity.CreatedDate, 0, botMessageEntity.Channel };
        return await DatabaseService.ExecuteNonResult(queryPayload);
    }

    private static async Task<bool> AddToDb(Item item, UserInfo user, string fileName, int lineNumber)
    {
        QueryPayload query = new QueryPayload
        {
            Sql = "INSERT INTO web_dupe_item(player_name, username, account_id, user_id, robot_id, primary_id, primary_type, second_id, second_type, created_date, line_number, file_name, is_valid, type) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
            Payload = new object[14]
            {
                user?.PlayerName ?? string.Empty,
                user?.Username ?? string.Empty,
                user?.AccountId ?? 0,
                user?.UserId ?? 0,
                item.RobotId,
                item.PrimaryId,
                item.PrimaryTypeId,
                item.SecondId,
                item.SecondTypeId,
                item.CreatedDate,
                lineNumber,
                fileName,
                item.IsValid ? '1' : '0',
                1
            }
        };
        bool num = await DatabaseService.ExecuteNonResult(query);
        if (!num)
        {
            _logger.Error((object)("Cannot insert to database -> " + JsonConvert.SerializeObject((object)query) + "]"));
        }
        return num;
    }

    private static async Task<bool> ItemValid(Item item, UserInfo user, string fileName, int lineNumber)
    {
        if (user == null)
        {
            return true;
        }
        QueryPayload queryPayload = new QueryPayload();
        queryPayload.Sql = "SELECT * FROM " + WebDupeItemEntity.GetTableName() + " WHERE second_id = ? AND second_type = ?";
        queryPayload.Payload = new object[2] { item.SecondId, item.SecondTypeId };
        if ((await DatabaseService.Execute<WebDupeItemEntity>(queryPayload)).Any())
        {
            _logger.Info((object)$"Detected user_id={user.UserId} username={user.Username} player_name={item.PlayerName} account_id={user.AccountId} item_id={item.SecondId} file_name={fileName} line_number={lineNumber} used item not existed");
            return false;
        }
        queryPayload = new QueryPayload();
        queryPayload.Sql = "SELECT * FROM " + WebDupeItemEntity.GetTableName() + " WHERE second_id = ? AND second_type = ?";
        queryPayload.Payload = new object[2] { item.PrimaryId, item.PrimaryTypeId };
        if ((await DatabaseService.Execute<WebDupeItemEntity>(queryPayload)).Any())
        {
            _logger.Info((object)$"Detected user_id={user.UserId} username={user.Username} player_name={item.PlayerName} account_id={user.AccountId} item_id={item.PrimaryId} file_name={fileName} line_number={lineNumber} used item not existed");
            return false;
        }
        queryPayload = new QueryPayload();
        queryPayload.Sql = "SELECT id FROM cq_itemex WHERE id = ?";
        queryPayload.Payload = new object[1] { item.SecondId };
        if ((await DatabaseService.Execute<ItemExEntity>(queryPayload)).Any())
        {
            _logger.Info((object)$"Detected user_id={user.UserId} username={user.Username} player_name={item.PlayerName} account_id={user.AccountId} item_id={item.PrimaryId} file_name={fileName} line_number={lineNumber} used item not existed");
            return false;
        }
        return true;
    }

    public static string Base64Encode(string plainText)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    private static async Task<int> GetLastLineNumber(string fileName)
    {
        QueryPayload queryPayload = new QueryPayload();
        queryPayload.Sql = "SELECT * FROM web_dupe_item d where d.file_name = ? order by id desc";
        queryPayload.Payload = new string[1] { fileName };
        return (await DatabaseService.Execute<WebDupeItemEntity>(queryPayload)).FirstOrDefault()?.LineNumber ?? 0;
    }

    private static async Task<UserInfo> GetUserInfo(Item item)
    {
        QueryPayload queryPayload = new QueryPayload();
        queryPayload.Sql = "select u.ip, a.online as is_banned, u.id as user_id, u.name as player_name, u.account_id, a.name as username from cq_user as u inner join account a on a.id = u.account_id where u.id = ?";
        queryPayload.Payload = new string[1] { item.RobotId };
        return (await DatabaseService.Execute<UserInfo>(queryPayload)).FirstOrDefault();
    }

    private static Item GetItems(string primary, string second)
    {
        Item item = GetItem(primary);
        Item item2 = GetItem(second);
        item.SecondId = item2.PrimaryId;
        item.SecondTypeId = item2.PrimaryTypeId;
        return item;
    }

    private static Item GetItem(string input)
    {
        string[] array = input.Split(new string[2] { ", ", "，" }, StringSplitOptions.RemoveEmptyEntries);
        string playerName = string.Empty;
        string robotId = string.Empty;
        string primaryId = string.Empty;
        string primaryTypeId = string.Empty;
        string[] array2 = array;
        foreach (string text in array2)
        {
            if (text.StartsWith("主法宝的    [玩家名字:") || text.StartsWith("辅助法宝的  [玩家名字:"))
            {
                playerName = text.Split(':').LastOrDefault();
            }
            if (text.StartsWith("玩家id="))
            {
                robotId = text.Split('=').LastOrDefault();
            }
            if (text.StartsWith("id="))
            {
                primaryId = text.Split('=').LastOrDefault();
            }
            if (text.StartsWith("typeid="))
            {
                primaryTypeId = text.Split('=').LastOrDefault();
            }
        }
        return new Item
        {
            CreatedDate = GetCreatedDate(input),
            RobotId = robotId,
            PrimaryId = primaryId,
            PrimaryTypeId = primaryTypeId,
            PlayerName = playerName
        };
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
}