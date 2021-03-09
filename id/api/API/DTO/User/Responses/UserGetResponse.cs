using API.DTO.Base;

namespace API.DTO.User.Responses
{
    public class UserGetResponse : StatusResponse
    {
        public int Id { get; set; }
        public string Branch { get; set; }
        public string BankPos { get; set; }
        public string LastLogin { get; set; }
        public string LastIpLogin { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
