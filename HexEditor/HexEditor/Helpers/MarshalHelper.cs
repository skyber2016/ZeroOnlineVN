using System;
using System.Runtime.InteropServices;

namespace HexEditor
{
    public static class MarshalHelper
    {
        public static byte[] StructToBytes(this object s, int size)
        {
            var bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(s, ptr, false);
                Marshal.Copy(ptr, bytes, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return bytes;
        }

        public static IntPtr StructToPointer(this object s, int size)
        {
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(s, ptr, false);
            return ptr;
        }

        public static IntPtr BytesToPointer(this byte[] bytes)
        {
            IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            return ptr.ToRelease();
        }
    }
}
