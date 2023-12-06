using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HexEditor
{
    [StructLayout(LayoutKind.Sequential)]
    public class ServerList
    {
        [JsonProperty("so_luong_may_chu")]
        public int TotalServers;

        [JsonIgnore]
        public IntPtr ServerPtr;

        public static ServerList Initialize(byte[] bytes)
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                var handlePtr = handle.AddrOfPinnedObject();
                var serverList = (ServerList)Marshal.PtrToStructure(handlePtr, typeof(ServerList));
                serverList.SetServerPtr(handlePtr);
                return serverList;
            }
            finally
            {
                handle.Free();
            }
        }

        private int Size => Marshal.SizeOf(typeof(ServerList)) - Marshal.SizeOf(typeof(IntPtr));

        public void SetServerPtr(IntPtr baseHandle)
        {
            this.ServerPtr = baseHandle + Size;
        }

        public IntPtr UpdateServer(params Server[] servers)
        {
            var bytes = new List<byte>();

            foreach (var server in servers)
            {
                bytes.AddRange(server.Build());
            }
            var retPtr = MarshalHelper.BytesToPointer(bytes.ToArray());
            if (this.ServerPtr != IntPtr.Zero)
            {
                this.ServerPtr.Release();
            }
            this.ServerPtr = retPtr;
            return retPtr;
        }
        [JsonProperty("may_chu")]
        public Server[] Servers
        {
            get
            {
                if (this.ServerPtr == IntPtr.Zero)
                {
                    return Array.Empty<Server>();
                }
                var servers = new List<Server>();
                var offsets = 0;
                for (int i = 0; i < TotalServers; i++)
                {
                    IntPtr serverPtr = IntPtr.Add(this.ServerPtr, offsets);
                    var server = (Server)Marshal.PtrToStructure(serverPtr, typeof(Server));
                    server.SetChannelPtr(serverPtr);
                    servers.Add(server);
                    offsets += server.Size + server.Channels.Sum(x => Marshal.SizeOf(x));

                }
                return servers.ToArray();
            }
            set
            {
                this.UpdateServer(value);
            }
        }

        public byte[] Build()
        {
            var builder = new List<byte>();
            var currentSize = MarshalHelper.StructToBytes(this, this.Size);
            builder.AddRange(currentSize);
            foreach (var server in Servers)
            {
                builder.AddRange(server.Build());
            }
            return builder.ToArray();
        }

    }


}
