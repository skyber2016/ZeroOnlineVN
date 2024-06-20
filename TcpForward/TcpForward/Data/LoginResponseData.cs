using System.Runtime.InteropServices;

namespace ServerForward.Data
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LoginResponseData
    {
        public ushort Length;
        public DataType Type;
        public int AccountId;
    }
}
