namespace Forum_API.Common
{
    public static class MessageConstants
    {
        public static readonly string UserNotMatch = "Invalid credential information. Please check again";
        public static readonly string UserNameExist = "User Name have been existed. Please check again";
        public static readonly string EmailExist = "Email have been existed.Please check again";
        public static readonly string EmailInvalid = "Invalid email. Please check again";
        public static readonly string LinkInvalid = "This link has expired";
        public static readonly string SameOldPassword = "Old password and new password cannot be the same. Please check again";
        public static readonly string OldPasswordNotCorrect = "Current password is incorret. Please check again";
        public static readonly string UserIsBlocked = "Your account is locked. Please contact admin for assistance";
        public static readonly string AccountIsBlocked = "Your account is being locked. Please contact admin for assistance";
        public static readonly string RoleExists = "Role Name have been existed. Please check again";
        public static readonly string QRExist = "This QR have existed. Please check again";
        public static readonly string QRExistBulk = "QR have existed: ";
        public static readonly string FileNull = "Request must not be null";
        public static readonly string FileInvalid = "Invalid File";
        public static readonly string BankNameExists = "Bank Name have been existed. Please check again";
        public static readonly string BankCodeExists = "Bank Code have been existed. Please check again";
        public static readonly string BankCodeIsNull = "Select the BANK CODE";
        public static readonly string QRExcelInvalid = "Please select the upload file with number of lines > 0 and <= 500";
    }
}
