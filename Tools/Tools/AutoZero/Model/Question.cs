using AutoAnswer.command;

namespace AutoAnswer.Model
{
    public class Question
    {
        public string Math { get; set; }
        public int Answer { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public byte[] FinalAnswer
        {
            get
            {
                if(this.Answer == this.A)
                {
                    return PacketContants.ANSWER_A;
                }
                if(this.Answer == this.B)
                {
                    return PacketContants.ANSWER_B;
                }
                if(this.Answer == this.C)
                {
                    return PacketContants.ANSWER_C;
                }
                return null;
            }
        }
    }
}
