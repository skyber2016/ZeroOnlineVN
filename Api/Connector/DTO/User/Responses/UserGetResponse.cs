using Forum_API.DTO.Role.Responses;

namespace Forum_API.DTO.User.Responses
{
    public class UserGetResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string RoleColor { get; set; }
    }
}
