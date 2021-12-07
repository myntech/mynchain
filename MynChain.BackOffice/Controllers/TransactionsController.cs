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
    public class TransactionsController : Controller
    {
        [Route("/Transactions/Home")]
        public IActionResult Home(string sortOrder)
        {
            MongoDb _data = new MongoDb();
            var _list = _data.GetCurrentTransactions();

            ViewBag.AmountSortParm = sortOrder == "Amount" ? "Amount_desc" : "Amount";
            ViewBag.RecipientSortParm = sortOrder == "Recipient" ? "Recipient_desc" : "Recipient";
            ViewBag.SenderSortParm = sortOrder == "Sender" ? "Sender_desc" : "Sender";

            switch (sortOrder)
            {
                case "Amount":
                    _list = _list.OrderBy(x => x.Amount).ToList();
                    break;
                case "Amount_desc":
                    _list = _list.OrderByDescending(x => x.Amount).ToList();
                    break;
                case "Recipient":
                    _list = _list.OrderBy(x => x.Recipient).ToList();
                    break;
                case "Recipient_desc":
                    _list = _list.OrderByDescending(x => x.Recipient).ToList();
                    break;
                case "Sender":
                    _list = _list.OrderBy(x => x.Sender).ToList();
                    break;
                case "Sender_desc":
                    _list = _list.OrderByDescending(x => x.Sender).ToList();
                    break;
                default:
                    _list.ToList();
                    break;
            }

            return View(_list);
        }

        [Route("/Transactions/Add")]
        public IActionResult Add()
        {
            return View();
        }

        [Route("/Transactions/Edit")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}