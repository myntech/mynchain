using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MynChain.Data;
using MynChain.Models;

/*DEVELOPED BY MYNTECH https://myntech.it*/
/*AUTHOR: MARCO AMATO*/

namespace MynChain.BackOffice.Controllers
{
    [SecurityHeaders]
    public class NodesController : Controller
    {
        [Route("/Nodes/Home")]
        public IActionResult Home(string sortOrder)
        {
            MongoDb _data = new MongoDb();
            var _list = _data.GetNodes();

            ViewBag.AddressSortParm = sortOrder == "Address" ? "Address_desc" : "Address";

            switch (sortOrder)
            {
                case "Address":
                    _list = _list.OrderBy(x => x.Address).ToList();
                    break;
                case "Address_desc":
                    _list = _list.OrderByDescending(x => x.Address).ToList();
                    break;
                default:
                    _list.ToList();
                    break;
            }

            return View(_list);
        }

        [HttpGet]
        [Route("/Nodes/Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromBody]Node node)
        {
            try
            {
                if (node.Address == null)
                {
                    return Json(new { success = false, responseText = "Address field is required." });
                }
                else if (String.IsNullOrEmpty(node.Address.ToString()))
                {
                    return Json(new { success = false, responseText = "Address field is required." });
                }
                else
                {
                    bool isValidUri = false;
                    Uri _uri;
                    isValidUri = Uri.TryCreate(node.Address.ToString(), UriKind.Absolute, out _uri);

                    if (isValidUri == false)
                    {
                        return Json(new { success = false, responseText = "The address is not a valid URI." });
                    }
                }

                MongoDb _data = new MongoDb();

                var exists = _data.CheckIfNodeExists(node.Address);
                if (exists == false)
                {
                    Node _node = new Node();
                    _node.Address = node.Address;
                    _data.AddNode(_node);

                    return Json(new { success = true, responseText = "Ok" });
                }
                else
                {
                    return Json(new { success = false, responseText = "The Address " + node.Address.ToString() + " already exists." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "There was an error. Please retry." });
            }
        }

        [HttpGet]
        [Route("/Nodes/Edit")]
        public IActionResult Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            MongoDb _data = new MongoDb();
            Node _node = _data.GetNodeById(id);

            return View(_node);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody]Node node)
        {
            if (String.IsNullOrEmpty(node.Id.ToString()))
            {
                return RedirectToAction("Home");
            }

            try
            {
                if (node.Address == null)
                {
                    return Json(new { success = false, responseText = "Address field is required." });
                }
                else if (String.IsNullOrEmpty(node.Address.ToString()))
                {
                    return Json(new { success = false, responseText = "Address field is required." });
                }
                else
                {
                    bool isValidUri = false;
                    Uri _uri;
                    isValidUri = Uri.TryCreate(node.Address.ToString(), UriKind.Absolute, out _uri);

                    if (isValidUri == false)
                    {
                        return Json(new { success = false, responseText = "The address is not a valid URI." });
                    }
                }

                MongoDb _data = new MongoDb();

                var exists = _data.CheckIfNodeExists(node.Address);
                if (exists == false)
                {
                    await _data.UpdateNode(node);

                    return Json(new { success = true, responseText = "Ok" });
                }
                else
                {
                    return Json(new { success = false, responseText = "The Address " + node.Address.ToString() + " already exists." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "There was an error. Please retry." });
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                MongoDb _data = new MongoDb();
                bool IsDeleted = await _data.DeleteNode(id);

                if (IsDeleted == true)
                {
                    //return Json(new { success = true, responseText = "Ok" });
                    return RedirectToAction("Home");
                }
                else
                {
                    return Json(new { success = false, responseText = "There was an error. Please retry." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "There was an error. Please retry." });
            }
        }
    }
}