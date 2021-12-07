using MynChain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain
{
    public class BlockChain
    {
        //private List<Transaction> _currentTransactions = new List<Transaction>();
        private List<Models.Block> _chain = new List<Models.Block>();
        //private List<Node> _nodes = new List<Node>();
        //private Block _lastBlock => _chain.Last();
        private Data.MongoDb _data = new Data.MongoDb();

        public string NodeId { get; private set; }

        //ctor
        public BlockChain()
        {
            NodeId = Guid.NewGuid().ToString().Replace("-", "");
            CreateGenesisBlock(proof: 100, previousHash: "1"); //genesis block
        }

        //private functionality
        private void RegisterNode(string address)
        {
            Node node = new Node();
            node.Address = new Uri(address);

            _data.AddNode(node);
        }

        public bool IsValidChain(List<Models.Block> chain)
        {
            Models.Block block = null;
            Models.Block lastBlock = chain.First();
            int currentIndex = 1;
            while (currentIndex < chain.Count)
            {
                block = chain.ElementAt(currentIndex);
                Debug.WriteLine($"{lastBlock}");
                Debug.WriteLine($"{block}");
                Debug.WriteLine("----------------------------");

                //Check that the hash of the block is correct
                if (block.PreviousHash != GetHash(lastBlock))
                    return false;

                //Check that the Proof of Work is correct
                if (!IsValidProof(lastBlock.Proof, block.Proof, lastBlock.PreviousHash))
                {
                    return false;
                }

                lastBlock = block;
                currentIndex++;
            }

            return true;
        }

        private bool ResolveConflicts()
        {
            List<Node> _nodes = _data.GetNodes();
            List<Models.Block> newChain = null;
            int maxLength = _data.GetNOfBlocks();

            foreach (Node node in _nodes)
            {
                var url = new Uri(node.Address, "/chain");
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var model = new
                    {
                        chain = new global::System.Collections.Generic.List<global::MynChain.Models.Block>(),
                        length = 0
                    };
                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var data = JsonConvert.DeserializeAnonymousType(json, model);

                    if (data.chain.Count > _chain.Count && IsValidChain(data.chain))
                    {
                        maxLength = data.chain.Count;
                        newChain = data.chain;
                    }
                }
            }

            if (newChain != null)
            {
                _chain = newChain;
                return true;
            }

            return false;
        }

        private Models.Block CreateGenesisBlock(int proof, string previousHash = null)
        {
            List<Models.Transaction> _currentTransactions = _data.GetCurrentTransactions();
            Models.Block lastBlock = _data.GetLastBlock();

            var block = new Models.Block
            {
                Index = _data.GetNOfBlocks(),
                Timestamp = DateTime.UtcNow,
                Transactions = _currentTransactions.ToList(),
                Proof = proof,
                PreviousHash = previousHash ?? GetHash(lastBlock)
            };

            _data.RemoveCurrentTransactions();

            _data.AddGenesisBlock(block);

            return block;
        }

        private Models.Block CreateNewBlock(int proof, string previousHash = null)
        {
            List<Models.Transaction> _currentTransactions = _data.GetCurrentTransactions();
            Models.Block lastBlock = _data.GetLastBlock();

            var block = new Models.Block
            {
                Index = _data.GetNOfBlocks(),
                Timestamp = DateTime.UtcNow,
                Transactions = _currentTransactions.ToList(),
                Proof = proof,
                PreviousHash = previousHash ?? GetHash(lastBlock)
            };

            _data.RemoveCurrentTransactions();

            _data.AddBlock(block);

            return block;
        }

        private int CreateProofOfWork(int lastProof, string previousHash)
        {
            int proof = 0;
            while (!IsValidProof(lastProof, proof, previousHash))
            {
                proof++;
            }

            return proof;
        }

        private bool IsValidProof(int lastProof, int proof, string previousHash)
        {
            string guess = $"{lastProof}{proof}{previousHash}";
            string result = GetSha256(guess);
            return result.StartsWith("0000");
        }

        private string GetHash(Models.Block block)
        {
            string blockText = JsonConvert.SerializeObject(block);
            return GetSha256(blockText);
        }

        private string GetSha256(string data)
        {
            var sha256 = new SHA256Managed();
            var hashBuilder = new StringBuilder();

            byte[] bytes = Encoding.Unicode.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);

            foreach (byte x in hash)
            {
                hashBuilder.Append($"{x:x2}");
            }

            return hashBuilder.ToString();
        }

        //API Calls
        public string Mine()
        {
            Models.Block _lastBlock = _data.GetLastBlock();
            int proof = CreateProofOfWork(_lastBlock.Proof, _lastBlock.PreviousHash);

            //CreateTransaction(sender: "0", recipient: NodeId, amount: 1);
            Models.Block block = CreateNewBlock(proof /*, _lastBlock.PreviousHash*/);

            var response = new
            {
                Message = "New Block Forged",
                Index = block.Index,
                Transactions = block.Transactions.ToArray(),
                Proof = block.Proof,
                PreviousHash = block.PreviousHash
            };

            return JsonConvert.SerializeObject(response);
        }

        public string GetFullChain()
        {
            var response = new
            {
                chain = _data.GetAllBlocks(),
                length = _data.GetNOfBlocks()
            };

            return JsonConvert.SerializeObject(response);
        }

        public string RegisterNodes(List<string> nodes)
        {
            var builder = new StringBuilder();
            foreach (string node in nodes)
            {
                string url = $"http://{node}";
                RegisterNode(url);
                builder.Append($"{url}, ");
            }

            builder.Insert(0, $"{nodes.Count()} new nodes have been added: ");
            string result = builder.ToString();
            return result.Substring(0, result.Length - 2);
        }

        public string Consensus()
        {
            bool replaced = ResolveConflicts();
            string message = replaced ? "was replaced" : "is authoritive";

            var response = new
            {
                Message = $"Our chain {message}",
                Chain = _chain
            };

            return JsonConvert.SerializeObject(response);
        }

        public int CreateTransaction(string sender, string recipient, int amount)
        {
            Models.Block _lastBlock = _data.GetLastBlock();

            var transaction = new Models.Transaction
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            };

            _data.AddTransaction(transaction);

            return _lastBlock != null ? _lastBlock.Index + 1 : 0;
        }

        public int CreateTransactionSmartContractAction(SmartContractAction action)
        {
            Models.Block _lastBlock = _data.GetLastBlock();

            var transaction = new Models.Transaction
            {
                Action = action
            };

            _data.AddTransaction(transaction);

            return _lastBlock != null ? _lastBlock.Index + 1 : 0;
        }
    }
}
