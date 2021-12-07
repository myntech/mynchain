using System;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain
{
    public interface IBlock
    {
        byte[] Data { get; }

        byte[] Hash { get; set; }

        int Nonce { get; set; }

        byte[] PrevHash { get; set; }

        DateTime TimeStamp { get; }
    }
}
