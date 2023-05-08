using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatManager.Models;

namespace ChatManager.Controllers
{
    public class ChatController : Controller
    {
        int SelectedFriendId
        {
            get
            {
                if (Session["selectedFriendId"] == null)
                    Session["selectedFriendId"] = -1;
                return (int)Session["selectedFriendId"];
            }
            set => Session["selectedFriendId"] = value;
        }

        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            ViewBag.User = OnlineUsers.GetSessionUser();
            ViewBag.SelectedFriendId = SelectedFriendId;
            return View();
        }
        public ActionResult FriendList()
        {
            return null;
        }
        public ActionResult ChatWindow()
        {
            return null;
        }

        public ActionResult SetCurrentTarget(int id)
        {
            SelectedFriendId = id;
            return null;
        }
    }
}