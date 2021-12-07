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
    public class SmartContractController : Controller
    {
        [HttpPost]
        [Route("smartcontract/add")]
        public bool Add([FromBody] SmartContract contract)
        {
            try
            {
                MongoDb _data = new MongoDb();
                _data.AddSmartContract(contract);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet]
        [Route("smartcontract/unprocessed")]
        public List<SmartContract> UnprocessedTransaction()
        {
            try
            {
                MongoDb _data = new MongoDb();
                List<SmartContract> _contracts = _data.GetSmartContracts();
                return _contracts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
