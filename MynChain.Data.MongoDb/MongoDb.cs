using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MynChain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Data
{
    public class MongoDb
    {
        public MongoClient _client;
        public IMongoDatabase db;

        public MongoDb()
        {
            Connect();
        }

        public void Connect()
        {
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress("localhost", 27017),
                UseSsl = false
            };

            _client = new MongoClient(settings);

            db = _client.GetDatabase("mynchain");
        }

        #region BLOCKS
        public async void AddGenesisBlock(Block block)
        {
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("blocks");
            var _count = collection.Count(new BsonDocument());

            if (_count <= 0)
            {
                var document = block.ToBsonDocument();
                await collection.InsertOneAsync(document);
            }
        }

        public async void AddBlock(Block block)
        {
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("blocks");
            var document = block.ToBsonDocument();
            await collection.InsertOneAsync(document);
        }

        public int GetNOfBlocks()
        {
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("blocks");
            var _count = collection.Count(new BsonDocument());
            return (int)_count;
        }

        public List<Block> GetAllBlocks()
        {
            List<Block> blocks = new List<Block>();

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("blocks");
            var _count = collection.Count(new BsonDocument());

            if (_count > 0)
            {
                var documents = collection.Find(_ => true).ToList();

                foreach (var block in documents)
                {
                    Block _block = new Block();
                    _block.Index = block.GetValue("Index").AsInt32;
                    _block.Proof = block.GetValue("Proof").AsInt32;
                    _block.PreviousHash = block.GetValue("PreviousHash").AsString;
                    blocks.Add(_block);
                }
            }

            return blocks;
        }

        public Block GetLastBlock()
        {
            Block block = new Block();

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("blocks");
            var _count = collection.Count(new BsonDocument());

            if(_count > 0)
            {
                var lastDocument = collection.Find(_ => true).SortByDescending(bson => bson["_id"]).First();

                block.Index = lastDocument.GetValue("Index").AsInt32;
                block.Proof = lastDocument.GetValue("Proof").AsInt32;
                block.PreviousHash = lastDocument.GetValue("PreviousHash").AsString;
            }

            return block;
        }
        #endregion

        #region NODES
        public async void AddNode(Node node)
        {
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("nodes");
            var document = node.ToBsonDocument();
            await collection.InsertOneAsync(document);
        }

        public async Task<bool> UpdateNode(Node node)
        {
            bool esito = false;

            var collection = db.GetCollection<Node>("nodes");

            var filter = Builders<Node>.Filter.Eq("_id", node.Id);
            var update = Builders<Node>.Update.Set("Address", node.Address);

            var result = await collection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            return esito;
        }

        public async Task<bool> DeleteNode(string id)
        {
            try
            {
                ObjectId _id = ObjectId.Parse(id);
                var collection = db.GetCollection<Node>("nodes");

                var delete = await collection.DeleteOneAsync(x => x.Id == _id);

                if (delete.DeletedCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<Node> GetNodes()
        {
            List<Node> nodes = new List<Node>();

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("nodes");
            var _count = collection.Count(new BsonDocument());

            if(_count > 0)
            {
                var documents = collection.Find(_ => true).ToList();

                foreach (var node in documents)
                {
                    Node _node = new Node();
                    _node.Id = node.GetValue("_id").AsObjectId;
                    _node.Address = new Uri(node.GetValue("Address").AsString);
                    nodes.Add(_node);
                }
            }

            return nodes;
        }

        public Node GetNodeById(string id)
        {
            ObjectId _id = ObjectId.Parse(id);
            var collection = db.GetCollection<Node>("nodes");

            Node _node = collection
                        .AsQueryable()
                        .SingleOrDefault(x => x.Id == _id);

            return _node;
        }

        public bool CheckIfNodeExists(Uri Address)
        {
            bool exists = false;

            var collection = db.GetCollection<Node>("nodes");

            var count = collection
                        .AsQueryable()
                        .Where(x => x.Address == Address)
                        .Count();

            if (count > 0)
            {
                exists = true;
            }

            return exists;
        }
        #endregion

        #region SMART CONTRACTS
        public async void AddSmartContract(SmartContract contract)
        {
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("contracts");
            var document = contract.ToBsonDocument();
            await collection.InsertOneAsync(document);
        }

        public List<SmartContract> GetSmartContracts()
        {
            List<SmartContract> contracts = new List<SmartContract>();

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("contracts");
            var _count = collection.Count(new BsonDocument());

            if (_count > 0)
            {
                var documents = collection.Find(_ => true).ToList();

                foreach (var contract in documents)
                {
                    SmartContract _contract = new SmartContract();
                    _contract.Id = contract.GetValue("_id").AsObjectId;
                    _contract.ProvisioningStatus = contract.GetValue("ProvisioningStatus").AsInt32;
                    _contract.DeployedByUserId = contract.GetValue("DeployedByUserId").AsString;
                    //_contract.Properties = contract.GetEnumerator("Properties").ToJson();
                    //_contract.Properties = contract.GetEnumerator("Transactions").ToJson();
                    contracts.Add(_contract);
                }
            }

            return contracts;
        }
        #endregion

        #region TRANSACTIONS
        public async void AddTransaction(Transaction transaction)
        {
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("transactions");
            var document = transaction.ToBsonDocument();
            await collection.InsertOneAsync(document);
        }

        public List<Models.Transaction> GetCurrentTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("transactions");
            var _count = collection.Count(new BsonDocument());

            if(_count > 0)
            {
                var documents = collection.Find(_ => true).ToList();

                foreach (var transaction in documents)
                {
                    Transaction _transaction = new Transaction();
                    _transaction.Id = transaction.GetValue("_id").AsObjectId;

                    if (transaction.TryGetValue("Amount", out BsonValue _amount) == true && transaction.GetValue("Amount") != BsonNull.Value)
                    {
                        _transaction.Amount = transaction.GetValue("Amount").AsInt32;
                    }

                    if (transaction.TryGetValue("Recipient", out BsonValue _recipient) == true && transaction.GetValue("Recipient") != BsonNull.Value)
                    {
                        _transaction.Recipient = transaction.GetValue("Recipient").AsString;
                    }

                    if (transaction.TryGetValue("Sender", out BsonValue _sender) == true && transaction.GetValue("Sender") != BsonNull.Value)
                    {
                        _transaction.Sender = transaction.GetValue("Sender").AsString;
                    }

                    if (transaction.TryGetValue("Action", out BsonValue _action) == true && transaction.GetValue("Action") != BsonNull.Value)
                    {
                        SmartContractAction _actionB = BsonSerializer.Deserialize<SmartContractAction>(transaction.GetValue("Action").ToBsonDocument());
                        _transaction.Action = _actionB;
                    }

                    transactions.Add(_transaction);
                }
            }

            return transactions;
        }

        public void RemoveCurrentTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("transactions");
            var _count = collection.Count(new BsonDocument());

            if(_count > 0)
            {
                var documents = collection.Find(_ => true).ToList();

                foreach (var transaction in documents)
                {
                    BsonDocument findPersonDoc = transaction;
                    collection.FindOneAndDelete(findPersonDoc);
                }
            }
        }

        public List<Transaction> GetUnprocessedTransaction()
        {
            List<Transaction> _trans = new List<Transaction>();
            var collection = db.GetCollection<Transaction>("transactions");

            var _transactions = collection
                        .AsQueryable();

            if (_transactions.Count() > 0)
            {
                var _transactionsL = collection
                            .AsQueryable()
                            .ToList();

                foreach (Transaction _transaction in _transactionsL)
                {
                    _trans.Add(_transaction);
                }
            }

            return _trans;
        }

        public Transaction GetUnprocessedTransactionById(string id)
        {
            ObjectId _id = ObjectId.Parse(id);
            var collection = db.GetCollection<Transaction>("transactions");

            Transaction _transaction = collection
                        .AsQueryable()
                        .SingleOrDefault(x => x.Id == _id);

            return _transaction;
        }

        public List<Transaction> GetUnprocessedTransactionBySender(string sender)
        {
            List<Transaction> _trans = new List<Transaction>();
            var collection = db.GetCollection<Transaction>("transactions");

            var _transactions = collection
                        .AsQueryable()
                        .Where(x => x.Sender == sender);

            if (_transactions.Count() > 0)
            {
                var _transactionsL = collection
                            .AsQueryable()
                            .Where(x => x.Sender == sender)
                            .ToList();

                foreach(Transaction _transaction in _transactionsL)
                {
                    _trans.Add(_transaction);
                }
            }
            
            return _trans;
        }

        public List<Transaction> GetUnprocessedTransactionByRecipient(string recipient)
        {
            List<Transaction> _trans = new List<Transaction>();
            var collection = db.GetCollection<Transaction>("transactions");

            var _transactions = collection
                        .AsQueryable()
                        .Where(x => x.Recipient == recipient);

            if (_transactions.Count() > 0)
            {
                var _transactionsL = collection
                            .AsQueryable()
                            .Where(x => x.Recipient == recipient)
                            .ToList();

                foreach (Transaction _transaction in _transactionsL)
                {
                    _trans.Add(_transaction);
                }
            }

            return _trans;
        }

        public List<Block> GetProcessedTransaction()
        {
            List<Block> _blocksL = new List<Block>();

            var collection = db.GetCollection<Block>("blocks");
            var _blocks = collection
                .AsQueryable();

            if (_blocks.Count() > 0)
            {
                foreach (Block _block in _blocks)
                {
                    var _transL = _block.Transactions
                        .ToList();

                    _block.Transactions = _transL;
                    _blocksL.Add(_block);
                }
            }

            return _blocksL;
        }

        public Block GetProcessedTransactionById(string id)
        {
            Block _blocksL = new Block();

            var collection = db.GetCollection<Block>("blocks");
            var filter = Builders<Block>.Filter.ElemMatch(x => x.Transactions, a => a.Id.ToString() == id);
            var _blocks = collection.Find(filter).ToList();

            if (_blocks.Count() > 0)
            {
                foreach (Block _block in _blocks)
                {
                    var _transL = _block.Transactions
                        .Where(x => x.Id.ToString() == id)
                        .ToList();

                    _block.Transactions = _transL;
                    _blocksL = _block;
                }
            }

            return _blocksL;
        }

        public List<Block> GetProcessedTransactionBySender(string sender)
        {
            List<Block> _blocksL = new List<Block>();

            var collection = db.GetCollection<Block>("blocks");
            var filter = Builders<Block>.Filter.ElemMatch(x => x.Transactions, a => a.Sender == sender);
            var _blocks = collection.Find(filter).ToList();

            if (_blocks.Count() > 0)
            {
                foreach (Block _block in _blocks)
                {
                    var _transL = _block.Transactions
                        .Where(x => x.Sender == sender)
                        .ToList();

                    _block.Transactions = _transL;
                    _blocksL.Add(_block);
                }
            }
            
            return _blocksL;
        }

        public List<Block> GetProcessedTransactionByRecipient(string recipient)
        {
            List<Block> _blocksL = new List<Block>();

            var collection = db.GetCollection<Block>("blocks");
            var filter = Builders<Block>.Filter.ElemMatch(x => x.Transactions, a => a.Recipient == recipient);
            var _blocks = collection.Find(filter).ToList();

            if (_blocks.Count() > 0)
            {
                foreach (Block _block in _blocks)
                {
                    var _transL = _block.Transactions
                        .Where(x => x.Recipient == recipient)
                        .ToList();

                    _block.Transactions = _transL;
                    _blocksL.Add(_block);
                }
            }

            return _blocksL;
        }
        #endregion
    }
}
