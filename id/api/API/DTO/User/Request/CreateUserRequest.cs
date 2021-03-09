namespace API.DTO.User.Request
{
    public class CreateUserRequest
    {
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public bool SupperAdmin { get; set; }
    }
}
