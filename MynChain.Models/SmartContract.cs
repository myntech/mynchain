using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Models
{
    public class SmartContract
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public int ProvisioningStatus { get; set; }

        [BsonElement]
        public string DeployedByUserId { get; set; }

        [BsonElement]
        public List<SmartContractProperty> Properties { get; set; }

        [BsonElement]
        public List<SmartContractAction> Actions { get; set; }
    }
}
