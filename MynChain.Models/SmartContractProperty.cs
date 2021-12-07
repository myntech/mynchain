using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Models
{
    public class SmartContractProperty
    {
        [BsonElement]
        public string Type { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string Value { get; set; }
    }
}
