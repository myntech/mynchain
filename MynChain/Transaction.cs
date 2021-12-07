using System;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain
{
    public class Transaction
    {
        public string Recipient { get; set; }

        public string Sender { get; set; }

        public DateTime TimeStamp { get; }
    }
}
