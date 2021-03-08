namespace Forum_API.Security
{
    public class UserPrincipal
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string RoleColor { get; set; }
        public long RoleId { get; set; }
        public int Prioritize { get; set; }
        public bool IsSystem => this.Prioritize == -1;
    }
}
