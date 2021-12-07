using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MynChain.Models;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Api.Controllers
{
    //[Authorize]
    public class ChainController : Controller
    {
        [HttpGet]
        [Route("chain")]
        public string Get()
        {
            try
            {
                return Startup.chain.GetFullChain();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpGet]
        [Route("chain/IsValidChain")]
        public bool IsValidChain([FromBody] List<Models.Block> blocks)
        {
            try
            {
                return Startup.chain.IsValidChain(blocks);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
