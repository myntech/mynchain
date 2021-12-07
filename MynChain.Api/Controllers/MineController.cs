using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Api.Controllers
{
    //[Authorize]
    [Route("mine")]
    public class MineController : Controller
    {
        [HttpGet]
        public string Get()
        {
            try
            {
                return Startup.chain.Mine();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
