using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MynChain.Data;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.BackOffice.Controllers
{
    [SecurityHeaders]
    public class BlocksController : Controller
    {
        [Route("/Blocks/Home")]
        public IActionResult Home(string sortOrder)
        {
            MongoDb _data = new MongoDb();
            var _list = _data.GetAllBlocks();

            ViewBag.IndexSortParm = sortOrder == "Index" ? "Index_desc" : "Index";
            ViewBag.PreviousHashSortParm = sortOrder == "PreviousHash" ? "PreviousHash_desc" : "PreviousHash";
            ViewBag.ProofSortParm = sortOrder == "Proof" ? "Proof_desc" : "Proof";
            ViewBag.TimestampSortParm = sortOrder == "Timestamp" ? "Timestamp_desc" : "Timestamp";

            switch (sortOrder)
            {
                case "Index":
                    _list = _list.OrderBy(x => x.Index).ToList();
                    break;
                case "Index_desc":
                    _list = _list.OrderByDescending(x => x.Index).ToList();
                    break;
                case "PreviousHash":
                    _list = _list.OrderBy(x => x.PreviousHash).ToList();
                    break;
                case "PreviousHash_desc":
                    _list = _list.OrderByDescending(x => x.PreviousHash).ToList();
                    break;
                case "Proof":
                    _list = _list.OrderBy(x => x.Proof).ToList();
                    break;
                case "Proof_desc":
                    _list = _list.OrderByDescending(x => x.Proof).ToList();
                    break;
                case "Timestamp":
                    _list = _list.OrderBy(x => x.Timestamp).ToList();
                    break;
                case "Timestamp_desc":
                    _list = _list.OrderByDescending(x => x.Timestamp).ToList();
                    break;
                default:
                    _list.ToList();
                    break;
            }

            return View(_list);
        }

        [Route("/Blocks/Add")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("/Blocks/Edit")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}