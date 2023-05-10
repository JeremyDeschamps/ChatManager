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
            return PartialView("FriendList", user.UsersWithFriendships.FindAll(u => u.StatusWith(user).Accepted));
        }
        public PartialViewResult ChatWindow()
        {
            ViewBag.Friend = DB.Users.FindUser(SelectedFriendId);
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
            User CurrentUser = OnlineUsers.GetSessionUser();
            Message newMessage = CurrentUser.MessagesSharedWith(SelectedFriendId).Find(m => m.Id == id);
            newMessage.Body = message;
            DB.Messages.Update(newMessage);
            return null;
        }
        public ActionResult Send(string message)
        {
            User CurrentUser = OnlineUsers.GetSessionUser();
            DB.Messages.Add(new Message { Body = message, IdSender = CurrentUser.Id, IdRecipient = SelectedFriendId, Seen = false, Date = DateTime.Now });
            return null;
        }
        public ActionResult Delete(int idMessage)
        {
            DB.Messages.Delete(idMessage);
            return null;
        }
    }
}