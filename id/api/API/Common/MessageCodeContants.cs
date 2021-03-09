namespace API.Common
{
    public static class MessageCodeContants
    {
        public static readonly string HOOKING_ERROR = "HOOKING_ERROR";
        public static readonly string USER_NOT_MATCH = "USER_NOT_MATCH";
        public static readonly string USERNAME_EXIST = "USERNAME_EXIST";
        public static readonly string EMAIL_EXIST = "EMAIL_EXIST";
        public static readonly string EmailInvalid = "Invalid email. Please check again";
        public static readonly string LinkInvalid = "This link has expired";
        public static readonly string SameOldPassword = "Old password and new password cannot be the same. Please check again";
        public static readonly string OldPasswordNotCorrect = "Current password is incorret. Please check again";
        public static readonly string USER_IS_BLOCKED = "USER_IS_BLOCKED";
        public static readonly string AccountIsBlocked = "Your account is being locked. Please contact admin for assistance";
        public static readonly string LOG_CREATE_USER = "LOG_CREATE_USER";
    }
}
