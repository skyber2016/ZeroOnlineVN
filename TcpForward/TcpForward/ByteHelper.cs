namespace ServerForward
{
    public static class ByteHelper
    {
        public static string vnToString(this byte[] bytes)
        {
            return string.Join(" ", bytes.Select(x => x.ToString("X")));
        }

        public static void WriteToFileHex(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public static void WriteToFileHex(this byte[] bytes, string connectionId, Channel channel)
        {
            var baseDir = Directory.GetCurrentDirectory();
            var dir = Path.Combine(baseDir, "DATA", channel.ToString(), connectionId);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var fileName = Path.Combine(dir, $"{DateTime.Now.ToString("yyyyMMddHHMMss")}.hex");
            File.WriteAllBytes(fileName, bytes);
        }
    }
}
