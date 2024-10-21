namespace GameServer.Packets
{
    public class LoginPacket : ISerializable
    {
        public uint ServerId {  get; set; }

        // len = 32
        public string Username {  get; set; }

        // len = 32
        public string Password { get; set; }
        // len = 32
        public string ServerName {  get; set; }
        // len = 32
        public string Version {  get; set; }
        public void Deserialize(byte[] buffer, int len)
        {
            using var mem = new MemoryStream(buffer);
            mem.Seek(0, SeekOrigin.Begin);
            using var reader = new BinaryReader(mem);
            reader.ReadBytes(4);
            this.ServerId = reader.ReadUInt32();
            this.Username = new string(reader.ReadChars(32)).Trim(char.MinValue);
            this.Password = new string(reader.ReadChars(32)).Trim(char.MinValue);
            this.ServerName = new string(reader.ReadChars(32)).Trim(char.MinValue);
            this.Version = new string(reader.ReadChars(32)).Trim(char.MinValue);
        }

        public byte[] Serialize()
        {
            using var mem = new MemoryStream();
            var writer = new BinaryWriter(mem);
            writer.Write(this.ServerId);
            writer.Write(this.Username.ToByteArray(32));
            writer.Write(this.Password.ToByteArray(32));
            writer.Write(this.ServerName.ToByteArray(32));
            writer.Write(this.Version.ToByteArray(32));
            return mem.ToArray();
        }
    }
}
