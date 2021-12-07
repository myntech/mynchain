using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MynChain.Models;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.Api.Controllers
{
    [Authorize]
    public class NodesController : Controller
    {
        [HttpGet]
        [Route("nodes/resolve")]
        public string Resolve()
        {
            try
            {
                return Startup.chain.Consensus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [HttpPost]
        [Route("nodes/register")]
        public string Register([FromBody] List<Node> nodes)
        {
            try
            {
                var _nodes = new List<string>();

                if (nodes != null)
                {
                    if (nodes.Count > 0)
                    {
                        foreach (Node _node in nodes)
                        {
                            _nodes.Add(_node.Address.ToString());
                        }
                    }
                }

                return Startup.chain.RegisterNodes(_nodes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
