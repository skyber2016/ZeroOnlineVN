using System.Collections.Generic;
using System.Linq;

namespace AutoAnswer.command
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

        public static byte[] ANSWER_A = new byte[] { 16, 0, 240, 7, 0, 0, 0, 0, 0, 0, 0, 101, 0, 0, 0, 0 };
        public static byte[] ANSWER_B = new byte[] { 16, 0, 240, 7, 0, 0, 0, 0, 0, 0, 1, 101, 0, 0, 0, 0 };
        public static byte[] ANSWER_C = new byte[] { 16, 0, 240, 7, 0, 0, 0, 0, 0, 0, 2, 101, 0, 0, 0, 0 };
        public static byte CONG = 0x2B;
        public static byte TRU = 0x2D;
        public static byte[] QUESTION_PARTERN = new byte[] { 0xF0, 0x07 };
        public static byte[] ANSWER_PARTERN = new byte[] { 0x14, 0x00, 0xF0, 0x07 };
        public static byte[] ANSWER_SEND_PARTERN = new byte[] { 16, 0, 240, 7 };
        public const int QESTION_LENGTH = 0x1E;
        public const int ANSWER_LENGTH = 0x14;
    }
}
