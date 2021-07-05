using System.Collections.Generic;

namespace NEWS_MVC.Security
{
    public class UserPrincipal
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string BranchCode { get; set; }
        public string PosCode { get; set; }
        public string UserLevel { get; set; }
        public bool ChangePassword { get; set; }
        public bool PassExpired { get; set; }
        public string Privileges { get; set; }
    }
}
