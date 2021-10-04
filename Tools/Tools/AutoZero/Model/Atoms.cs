namespace AutoAnswer.Model
{
    public class Atoms
    {
        public byte[] Packet { get; set; }
        public bool IsUse { get; set; }
        public int Retries { get; set; } = 0;
    }
}
