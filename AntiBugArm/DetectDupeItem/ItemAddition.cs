using DetectDupeItem.Models;
using DetectDupeItem.Services;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DetectDupeItem
{
    internal static class ItemAddition
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Encoding _encoding;
        
        public static async Task Tracking(FileSystemEventArgs e)
        {
            if(_encoding == null)
            {
                _encoding = Encoding.GetEncoding("GB2312");
            }
            var pathToFile = e.FullPath;
            var lines = File.ReadAllLines(pathToFile, _encoding);
            var lastLine = await GetLastLineNumber(e);
            for (int i = lastLine; i < lines.Length; i++)
            {
                var input = lines[i];
                var item = GetItem(input);
                UserInfo user = null;
                try
                {
                    user = await GetUserInfo(item);
                    if(user == null)
                    {
                        continue;
                    }
                    item.IsValid = await ItemValid(item, user, e, i + 1);
                    if (!item.IsValid)
                    {
                        _logger.Error($"user_id={user.UserId} username={user.Username} robot_id={item.RobotId} account_id={user.AccountId} item_id={item.SecondId} file_name={e.Name} line_number={i + 1}");
                        if(user != null)
                        {
                            if (!user.AccountHasBanned())
                            {
                                var noti = GetNotification(item, user, e, i + 1);
                                _logger.Info(noti);
                                await SendNotification(noti);

                                if (await user.Banned())
                                {
                                    WinService.BlockIP(user.IP);
                                    await Task.Delay(10000);
                                    WinService.UnblockIP(user.IP);
                                    _logger.Info($"Account {user.Username} has been banned");
                                }
                                else
                                {
                                    _logger.Info($"Account {user.Username} blocking with error");
                                }
                            }
                            else
                            {
                                _logger.Info($"{user?.Username} has been banned");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.GetBaseException().Message);
                    _logger.Error(ex.GetBaseException().StackTrace);
                }
                finally
                {
                    await AddToDb(item, user, e, i + 1);
                }
                
            }
        }

        static async Task<UserInfo> GetUserInfo(Item item)
        {
            var resp = await DatabaseService.Execute<UserInfo>(new QueryPayload
            {
                Sql = "select u.ip, a.online as is_banned, u.id as user_id, u.name as player_name, u.account_id, a.name as username, r.id as robot_id from cq_robot as r  inner join cq_user u on u.id = r.player_id inner join account a on a.id = u.account_id where r.id = ?",
                Payload = new string[] { item.RobotId }
            });
            var info = resp.FirstOrDefault();
            return info;
        }

        static async Task<bool> AddToDb(Item item, UserInfo user, FileSystemEventArgs e, int lineNumber)
        {
            var query = new QueryPayload
            {
                Sql = $"INSERT INTO web_dupe_item(player_name, username, account_id, user_id, robot_id, primary_id, primary_type, second_id, second_type, created_date, line_number, file_name, is_valid) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?)",
                Payload = new object[] { user?.PlayerName ?? string.Empty, user?.Username ?? string.Empty, user?.AccountId ?? 0, user?.UserId ?? 0, item.RobotId, item.PrimaryId, item.PrimaryTypeId, item.SecondId, item.SecondTypeId, item.CreatedDate, lineNumber, e.Name, item.IsValid ? '1' : '0' }
            };
            var isSuccess = await DatabaseService.ExecuteNonResult(query);
            if (!isSuccess)
            {
                _logger.Error($"Cannot insert to database -> {JsonConvert.SerializeObject(query)}]");
            }
            return isSuccess;
        }

        static string GetNotification(Item item, UserInfo user, FileSystemEventArgs e, int lineNumber)
        {
            var padRight = 20;
            var noti = new StringBuilder();
            noti.AppendLine("--------------- CẢNH BÁO GIAN LẬN ---------------");
            noti.Append($"**Tên đăng nhập:**".PadRight(padRight)).AppendLine(user?.Username ?? "Không xác định");
            noti.Append($"**Tên nhân vật:**".PadRight(padRight)).AppendLine(user?.PlayerName.ToString() ?? "Không xác định");
            noti.Append($"**Account_ID:**".PadRight(padRight)).AppendLine(user?.AccountId.ToString() ?? "Không xác định");
            noti.Append($"**User_ID:**".PadRight(padRight)).AppendLine(user?.UserId.ToString() ?? "Không xác định");
            noti.Append($"**Robot_ID:**".PadRight(padRight)).AppendLine(item.RobotId.ToString() ?? "Không xác định");
            noti.Append($"**Item chính:**".PadRight(padRight)).AppendLine(item.PrimaryId.ToString() ?? "Không xác định");
            noti.Append($"**Loại item chính:**".PadRight(padRight)).AppendLine(item.PrimaryTypeId.ToString() ?? "Không xác định");
            noti.Append($"**Item phụ:**".PadRight(padRight)).AppendLine(item.SecondId.ToString() ?? "Không xác định");
            noti.Append($"**Loại item phụ:**".PadRight(padRight)).AppendLine(item.SecondTypeId.ToString() ?? "Không xác định");
            noti.Append($"**Thời gian:**".PadRight(padRight)).AppendLine(item.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") ?? "Không xác định");
            noti.Append($"**Nguyên nhân:**".PadRight(padRight)).AppendLine($"Sử dụng item [id={item.SecondId} type={item.SecondTypeId}] trùng lặp do hack/bug/dupe");
            return noti.ToString();
        }

        static async Task<bool> SendNotification(string message)
        {
            var ent = new BotMessageEntity
            {
                Channel = "1125312849657483275",
                CreatedDate = DateTime.Now,
                IsSent = false,
                Message = Base64Encode(message)
            };
            var isSuccess = await DatabaseService.ExecuteNonResult(new QueryPayload
            {
                Sql = "INSERT INTO bot_message(message, created_date, is_sent, channel) VALUES(?,?,?,?)",
                Payload = new object[] { ent.Message, ent.CreatedDate, 0, ent.Channel }
            });
            return isSuccess;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        static async Task<int> GetLastLineNumber(FileSystemEventArgs e)
        {
            var listItems = await DatabaseService.Execute<WebDupeItemEntity>(new QueryPayload
            {
                Sql = "SELECT * FROM web_dupe_item d where d.file_name = ? order by id desc",
                Payload = new string[] { e.Name }
            });
            return listItems.FirstOrDefault()?.LineNumber ?? 0;
        }

        static async Task<bool> ItemValid(Item item, UserInfo user, FileSystemEventArgs e, int lineNumber)
        {
            if(user == null)
            {
                return true;
            }
            var query = new QueryPayload
            {
                Sql = $"SELECT * FROM {WebDupeItemEntity.GetTableName()} WHERE second_id = ? AND second_type = ?",
                Payload = new object[] { item.SecondId, item.SecondTypeId }
            };
            var results = await DatabaseService.Execute<WebDupeItemEntity>(query);
            if (results.Any())
            {
                _logger.Info($"Detected user_id={user.UserId} username={user.Username} robot_id={item.RobotId} account_id={user.AccountId} item_id={item.SecondId} file_name={e.Name} line_number={lineNumber} used item not existed");
                return false;
            }
            return true;
        }

        private static Item GetItem(string input)
        {
            // Tạo biểu thức chính quy để tìm các giá trị
            string pattern = @"\[(\d+)\]";

            // Tìm tất cả các kết quả phù hợp với biểu thức chính quy
            MatchCollection matches = Regex.Matches(input, pattern);
            var id = matches[0].Groups[1].Value;
            var itemSource = matches[1].Groups[1].Value;
            var itemSourceType = matches[2].Groups[1].Value;
            var itemDes = matches[3].Groups[1].Value;
            var itemDesType = matches[4].Groups[1].Value;
            var createdDate = GetCreatedDate(input);
            return new Item
            {
                RobotId = id,
                PrimaryId = itemSource,
                PrimaryTypeId = itemSourceType,
                SecondId = itemDes,
                SecondTypeId = itemDesType,
                CreatedDate = createdDate
            };
        }

        private static DateTime GetCreatedDate(string input)
        {
            string dateString = input.Split(new string[] { "--" }, StringSplitOptions.None).LastOrDefault();
            string format = "ddd MMM dd HH:mm:ss yyyy";
            if (DateTime.TryParseExact(dateString?.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            else
            {
                _logger.Error($"Cannot convert {dateString} to DateTime");
                return DateTime.Now;
            }
        }
    }
}
