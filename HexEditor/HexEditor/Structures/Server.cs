using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HexEditor
{
    [StructLayout(LayoutKind.Sequential)]
    public class Server
    {
        [JsonProperty("ma_may_chu")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Code;

        [JsonProperty("hinh_anh")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string UIPath;

        [JsonProperty("ten_may_chu")]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DisplayName;

        [JsonProperty("so_luong_kenh_login")]
        public int TotalChannels;

        [JsonIgnore]
        public IntPtr ChannelPtr;

        [JsonIgnore]
        public int Size => Marshal.SizeOf(typeof(Server)) - Marshal.SizeOf(typeof(IntPtr));

        public void SetChannelPtr(IntPtr baseHandle)
        {
            this.ChannelPtr = baseHandle + this.Size;
        }

        public IntPtr UpdateChannel(params Channel[] channels)
        {
            var bytes = channels.SelectMany(x => x.Build());
            var retPtr = MarshalHelper.BytesToPointer(bytes.ToArray());
            if (this.ChannelPtr != IntPtr.Zero)
            {
                this.ChannelPtr.Release();
            }
            this.ChannelPtr = retPtr;
            return retPtr;
        }

        public byte[] Build()
        {
            var builder = new List<byte>();
            var currentBytes = MarshalHelper.StructToBytes(this, this.Size);
            builder.AddRange(currentBytes);
            foreach (var channel in Channels)
            {
                builder.AddRange(channel.Build());
            }
            return builder.ToArray();
        }

        [JsonProperty("kenh_dang_nhap")]
        public Channel[] Channels
        {
            get
            {
                if (this.ChannelPtr == IntPtr.Zero)
                {
                    return Array.Empty<Channel>();
                }
                try
                {
                    var channels = new List<Channel>();
                    for (int i = 0; i < TotalChannels; i++)
                    {
                        IntPtr channelPtr = IntPtr.Add(this.ChannelPtr, i * Marshal.SizeOf(typeof(Channel)));
                        channels.Add((Channel)Marshal.PtrToStructure(channelPtr, typeof(Channel)));
                    }
                    return channels.ToArray();
                }
                catch (Exception)
                {
                    return Array.Empty<Channel>();
                }

            }
            set
            {
                this.UpdateChannel(value);
            }
        }
    }
}
