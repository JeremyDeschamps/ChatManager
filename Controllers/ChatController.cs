using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
            return View();
        }
        public PartialViewResult FriendList()
        {
            User user = OnlineUsers.GetSessionUser();
            ViewBag.User = user;
            ViewBag.SelectedFriendId = SelectedFriendId;
            return PartialView("FriendList", user.Friends.FindAll(u => u.StatusWith(user).Accepted));
        }
        public PartialViewResult ChatWindow()
        {
            return PartialView("ChatWindow");
        }

        public ActionResult SetCurrentTarget(int id)
        {
            SelectedFriendId = id;
            return null;
        }

        public ActionResult IsTargetTyping()
        {
            //TODO: 
            if (true)
                return Content("show=true");
            return Content("");
        }
        public ActionResult IsTyping()
        {
            return null;
        }
        public ActionResult StopTyping()
        {
            return null;
        }
        public ActionResult Update(int id, string message)
        {
            return null;
        }
        public ActionResult Send(string message)
        {
            return null;
        }
        public ActionResult Delete(int idMessage)
        {
            //fonctionnera pas, requête sous forme /Chat/Delete/{id}
            return null;
        }
    }
}