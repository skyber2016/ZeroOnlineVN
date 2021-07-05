namespace CORE_API.Configurations
{
    public class JwtSetting
    {
        public string SecretKey { get; set; }
        public int Expire { get; set; }
        public int ExpireRefreshToken { get; set; }
        public int ExpireIfNotInteractive { get; set; }
    }
}
