using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace API.Common
{
    public enum Message
    {
        NotNull,
        UserNotMatch,
        UserNameExist,
        UserNameNotNull,
        EmailExist,
        EmailInvalid,
        LinkInvalid,
        SameOldPassword,
        OldPasswordNotCorrect,
        PasswordNotNull,
        UserIsBlocked,
        AccountIsBlocked,
        RoleExists,
        QRExist,
        QRExistBulk,
        FileNull,
        FileInvalid,
        BankNameExists,
        BankCodeExists,
        BankCodeIsNull,
        QRExcelInvalid,
        RateLimit,
        RoleNameMaxLength,
        RoleDescriptionMaxLength,
        RoleNameNotNull,
        MinLength,
        MaxLength
}
    public static class MessageConstants
    {
        private static JObject Messages { get; set; }

        public static string GetMessage(Message messageType)
        {
            if (Messages == null)
            {
                Messages = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("Message.json"));
            }
            var messageTypeStr = messageType.ToString();
            return Messages.Value<string>(messageTypeStr);
        }
    }
}
