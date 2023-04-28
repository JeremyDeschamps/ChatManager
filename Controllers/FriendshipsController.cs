using ChatManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipsController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendFriendRequest() 
        {
            if(ModelState.IsValid)
            {
                RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult GetFriendShipsStatus()
        {
            return View();
        }
       /* public ActionResult Filter(bool check)
        {
            return View(check);
        }*/
    }
}