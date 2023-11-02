using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HexEditor
{
    public static class IntPtrHelper
    {
        private static List<IntPtr> _list = new List<IntPtr>();
        public static IntPtr ToRelease(this IntPtr ptr)
        {
            _list.Add(ptr);
            return ptr;
        }

        public static void Release(this IntPtr intPtr)
        {
            Marshal.FreeHGlobal(intPtr);
        }

        public static void Release()
        {
            foreach (var ptr in _list)
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
