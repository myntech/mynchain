using System;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain
{
    public class Block : IBlock
    {
        public Block(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Nonce = 0;
            PrevHash = new byte[] { 0x00 };
            TimeStamp = DateTime.Now;
        }

        public byte[] Data { get; }

        public byte[] Hash { get; set; }

        public int Nonce { get; set; }

        public byte[] PrevHash { get; set; }

        public DateTime TimeStamp { get; }

        public override string ToString()
        {
            return $"{BitConverter.ToString(Hash).Replace("-", "")};\n{BitConverter.ToString(PrevHash).Replace("-", "")}\n {Nonce} {TimeStamp}";
        }
    }
}
