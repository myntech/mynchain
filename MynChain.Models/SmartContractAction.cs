using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Models
{
    public class SmartContractAction
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string UserId{ get; set; }

        [BsonElement]
        public int ProvisioningStatus { get; set; }

        [BsonElement]
        public DateTime Timestamp { get; set; }

        [BsonElement]
        public string Value { get; set; }
    }
}
