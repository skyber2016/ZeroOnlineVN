
using Newtonsoft.Json;

namespace Forum_API.DTO.Base
{
    public class BaseUserGetResponse
    {
        [JsonProperty("FullName")]
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string RoleColor { get; set; }
        public string RoleClasses { get; set; }
    }
}
