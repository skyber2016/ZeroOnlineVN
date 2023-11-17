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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal static class ItemAddition
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
        Console.WriteLine($"Begin tracking {fileName}");
        string[] lines = await GmLogService.GetLines(fileName, _encoding);
        var lastLineNumber = await GetLastLineNumber(fileName);
        for (int i = lastLineNumber; i < lines.Length; i++)
        {
            Console.WriteLine(lines[i]);
            _logger.Info($"Tracking {lines[i]}");
            string input = lines[i];
            Item item = GetItem(input);
            UserInfo user = null;
            try
            {
                try
                {
                    user = await GetUserInfo(item);
                    if (user == null)
                    {
                        continue;
                    }
                    item.IsValid = await ItemValid(item, user, fileName, i + 1);
                    if (item.IsValid)
                    {
                        continue;
                    }
                    _logger.Error((object)$"user_id={user.UserId} username={user.Username} robot_id={item.RobotId} account_id={user.AccountId} item_id={item.SecondId} file_name={fileName} line_number={i + 1}");
                    if (user == null)
                    {
                        continue;
                    }
                    if (!user.AccountHasBanned())
                    {
                        string notification = GetNotification(item, user, fileName, i + 1);
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
                    }
                    else
                    {
                        _logger.Info((object)(user?.Username + " has been banned"));
                    }
                    continue;

                }
                catch (Exception ex)
                {
                    _logger.Error((object)ex.GetBaseException().Message);
                    _logger.Error((object)ex.GetBaseException().StackTrace);

                }
            }
            finally
            {
                await AddToDb(item, user, fileName, i + 1);
            }
        }
    }

    private static async Task<UserInfo> GetUserInfo(Item item)
    {
        QueryPayload queryPayload = new QueryPayload();
        queryPayload.Sql = "select u.ip, a.online as is_banned, u.id as user_id, u.name as player_name, u.account_id, a.name as username, r.id as robot_id from cq_robot as r  inner join cq_user u on u.id = r.player_id inner join account a on a.id = u.account_id where r.id = ?";
        queryPayload.Payload = new string[1] { item.RobotId };
        return (await DatabaseService.Execute<UserInfo>(queryPayload)).FirstOrDefault();
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
                0
            }
        };
        bool num = await DatabaseService.ExecuteNonResult(query);
        if (!num)
        {
            _logger.Error((object)("Cannot insert to database -> " + JsonConvert.SerializeObject((object)query) + "]"));
        }
        return num;
    }

    private static string GetNotification(Item item, UserInfo user, string fileName, int lineNumber)
    {
        int totalWidth = 20;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("--------------- CẢNH BÁO GIAN LẬN ---------------");
        stringBuilder.Append("**Loại:**".PadRight(totalWidth)).AppendLine("Cộng trang bị");
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
            _logger.Info((object)$"Detected user_id={user.UserId} username={user.Username} robot_id={item.RobotId} account_id={user.AccountId} item_id={item.SecondId} file_name={fileName} line_number={lineNumber} used item not existed");
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

    private static Item GetItem(string input)
    {
        string pattern = "\\[(\\d+)\\]";
        MatchCollection matchCollection = Regex.Matches(input, pattern);
        string value = matchCollection[0].Groups[1].Value;
        string value2 = matchCollection[1].Groups[1].Value;
        string value3 = matchCollection[2].Groups[1].Value;
        string value4 = matchCollection[3].Groups[1].Value;
        string value5 = matchCollection[4].Groups[1].Value;
        DateTime createdDate = GetCreatedDate(input);
        return new Item
        {
            RobotId = value,
            PrimaryId = value2,
            PrimaryTypeId = value3,
            SecondId = value4,
            SecondTypeId = value5,
            CreatedDate = createdDate
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