using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Models
{
    public class Block
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public int Index { get; set; }

        [BsonElement]
        public DateTime Timestamp { get; set; }

        [BsonElement]
        public List<Transaction> Transactions { get; set; }

        [BsonElement]
        public int Proof { get; set; }

        [BsonElement]
        public string PreviousHash { get; set; }

        public override string ToString()
        {
            return $"{Index} [{Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}] Proof: {Proof} | PrevHash: {PreviousHash} | Trx: {Transactions.Count}";
        }
    }
}