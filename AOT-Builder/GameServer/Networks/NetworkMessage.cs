namespace GameServer
{
    public class NetworkMessage : ISerializable
    {
        public ushort Len { get;set; }
        public PacketType Type { get;set; }
        public byte[] Data { get; set; }


        public byte[] Serialize()
        {
            using var mem = new MemoryStream();
            using var bw = new BinaryWriter(mem);
            bw.Write(Len);
            bw.Write((uint)Type);
            bw.Write(Data);
            return mem.ToArray();
        }

        public void Deserialize(byte[] buffer, int len)
        {
            using var mem = new MemoryStream(buffer);
            mem.Seek(0, SeekOrigin.Begin);
            var reader = new BinaryReader(mem);
            Len = reader.ReadUInt16();
            Type = (PacketType)reader.ReadUInt16();
            Data = reader.ReadBytes(len - 4);
        }
    }
}
