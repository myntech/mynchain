using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Models
{
    public class Transaction
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public int Amount { get; set; }

        [BsonElement]
        public string Recipient { get; set; }

        [BsonElement]
        public string Sender { get; set; }

        [BsonElement]
        public SmartContractAction Action { get; set; }
    }
}