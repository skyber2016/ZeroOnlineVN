namespace GameServer
{
    public interface ISerializable
    {
        public byte[] Serialize();
        public void Deserialize(byte[] buffer, int len);
    }
}
