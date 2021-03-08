using System.Collections.Generic;

namespace Forum_API.DTO.Authenticate.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public IDictionary<string,bool> Privileges{ get; set; }
    }
}
