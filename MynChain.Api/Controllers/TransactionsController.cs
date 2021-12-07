using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MynChain.Models;
using MynChain.Data;
using Microsoft.AspNetCore.Authorization;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Api.Controllers
{
    //[Authorize]
    public class TransactionsController : Controller
    {
        [HttpPost]
        [Route("transactions/add")]
        public string Add([FromBody] Models.Transaction transaction)
        {
            try
            {
                int blockId = Startup.chain.CreateTransaction(transaction.Sender, transaction.Recipient, transaction.Amount);
                return $"Your transaction will be included in block {blockId}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("transactions/add/smartcontractaction")]
        public string AddSmartContractAction([FromBody] SmartContractAction action)
        {
            try
            {
                int blockId = Startup.chain.CreateTransactionSmartContractAction(action);
                return $"Your transaction will be included in block {blockId}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet]
        [Route("transactions/unprocessed")]
        public List<Models.Transaction> UnprocessedTransaction()
        {
            try
            {
                MongoDb _data = new MongoDb();
                List<Models.Transaction> _transactions = _data.GetUnprocessedTransaction();
                return _transactions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("transactions/unprocessed/byId")]
        public Models.Transaction UnprocessedTransactionById([FromBody] Models.Transaction transaction)
        {
            try
            {
                MongoDb _data = new MongoDb();
                Models.Transaction _transaction = _data.GetUnprocessedTransactionById(transaction.Id.ToString());
                return _transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("transactions/unprocessed/bySender")]
        public List<Models.Transaction> UnprocessedTransactionBySender([FromBody] Models.Transaction transaction)
        {
            try
            {
                MongoDb _data = new MongoDb();
                List<Models.Transaction> _transactions = _data.GetUnprocessedTransactionBySender(transaction.Sender.ToString());
                return _transactions;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("transactions/unprocessed/byRecipient")]
        public List<Models.Transaction> UnprocessedTransactionByRecipient([FromBody] Models.Transaction transaction)
        {
            try
            {
                MongoDb _data = new MongoDb();
                List<Models.Transaction> _transactions = _data.GetUnprocessedTransactionByRecipient(transaction.Recipient.ToString());
                return _transactions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet]
        [Route("transactions/processed")]
        public List<Models.Block> ProcessedTransaction()
        {
            try
            {
                MongoDb _data = new MongoDb();
                List<Models.Block> _blocks = _data.GetProcessedTransaction();
                return _blocks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("transactions/processed/byId")]
        public Models.Block ProcessedTransactionById([FromBody] Models.Transaction transaction)
        {
            try
            {
                MongoDb _data = new MongoDb();
                Models.Block block = _data.GetProcessedTransactionById(transaction.Id.ToString());
                return block;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("transactions/processed/bySender")]
        public List<Models.Block> ProcessedTransactionBySender([FromBody] Models.Transaction transaction)
        {
            try
            {
                MongoDb _data = new MongoDb();
                List<Models.Block> _blocks = _data.GetProcessedTransactionBySender(transaction.Sender.ToString());
                return _blocks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("transactions/processed/byRecipient")]
        public List<Models.Block> ProcessedTransactionByRecipient([FromBody] Models.Transaction transaction)
        {
            try
            {
                MongoDb _data = new MongoDb();
                List<Models.Block> _blocks = _data.GetProcessedTransactionByRecipient(transaction.Recipient.ToString());
                return _blocks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
