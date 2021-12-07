using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Models
{
    public class Node
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public Uri Address { get; set; }
    }
}