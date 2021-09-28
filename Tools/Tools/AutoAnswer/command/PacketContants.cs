using System.Collections.Generic;
using System.Linq;

namespace MiddlewareTCP.command
{
    public static class PacketContants
    {
        public static byte[] ARM
        {
            get
            {
                return new byte[] { 0x5B, 0x04 };
            }
        }

        public static byte[] LoginTypeRequest
        {
            get
            {
                return new byte[] { 0x42, 0x04, 0x05, 0x0, 0x0, 0x0 };
            }
        }
        

        public static byte[] LoginResponse
        {
            get
            {
                return new byte[] { 31, 4 };
            }
        }

        public static byte[] JoinGameRequest
        {
            get
            {
                return new byte[] { 28, 4 };
            }
        }

        public static byte[] AttackNormal
        {
            get
            {
                return new byte[] { 0xFE, 0x03 };
            }
        }

        public static byte[] GetPacketType(this byte[] data, int length = 6)
        {
            return data.Skip(2).Take(length).ToArray();
        }

        public static bool vnEquals(this byte[] source, byte[] des)
        {
            return source.Split().Equals(des.Split());
        }
        public static Dictionary<string, string> SecretKey = new Dictionary<string, string>();
    }
}
