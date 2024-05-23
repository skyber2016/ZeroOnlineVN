namespace BotDiscordNew
{
    public enum Connection
    {
        Main, Sub
    }
    public  class MultipleSetting
    {
        public ulong Channel {  get; set; }
        public Connection UseConnection {  get; set; }
    }
}
