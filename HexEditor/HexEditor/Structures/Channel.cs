using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace HexEditor
{
    [StructLayout(LayoutKind.Sequential)]
    public class Channel
    {
        [JsonProperty("ip")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string IPAddress;

        [JsonProperty("ma_kenh")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string ServerName;

        [JsonProperty("hinh_anh")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string UIPath;

        [JsonProperty("ten_kenh")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DisplayName;

        [JsonProperty("port")]
        public int LoginPort;

        public byte[] Build() => MarshalHelper.StructToBytes(this, Marshal.SizeOf(typeof(Channel)));
        
    }

}
