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
            ViewBag.IsAdmin = OnlineUsers.GetSessionUser().IsAdmin;
            return View();
        }
        [OnlineUsers.UserAccess(false)]
        public ActionResult FriendList()
        {
            User user = OnlineUsers.GetSessionUser();
            ViewBag.User = user;
            ViewBag.SelectedFriendId = SelectedFriendId;
            return PartialView("FriendList", user.UsersWithFriendships.FindAll(u => u.StatusWith(user).Accepted));
        }
        [OnlineUsers.UserAccess(false)]
        public ActionResult ChatWindow(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Messages.HasChanged)
            {
                ViewBag.Friend = DB.Users.FindUser(SelectedFriendId);
                User user = OnlineUsers.GetSessionUser();
                return PartialView("ChatWindow", user.MessagesSharedWith(SelectedFriendId));
            }
            return null;
        }
        [OnlineUsers.UserAccess]
        public ActionResult SetCurrentTarget(int id)
        {
            SelectedFriendId = id;
            return null;
        }
        [OnlineUsers.UserAccess]
        public ActionResult IsTargetTyping()
        {
            //TODO: 
            if (true)
                return Content("show=true");
        }
        [OnlineUsers.UserAccess]
        public ActionResult IsTyping()
        {
            return null;
        }
        [OnlineUsers.UserAccess]
        public ActionResult StopTyping()
        {
            return null;
        }
        [OnlineUsers.UserAccess]
        public ActionResult Update(int id, string message)
        {
            User CurrentUser = OnlineUsers.GetSessionUser();
            Message newMessage = CurrentUser.MessagesSharedWith(SelectedFriendId).Find(m => m.Id == id);
            newMessage.Body = message;
            DB.Messages.Update(newMessage);
            return null;
        }
        [OnlineUsers.UserAccess]
        public ActionResult Send(string message)
        {
            User CurrentUser = OnlineUsers.GetSessionUser();
            if(SelectedFriendId > 0)
            {
                DB.Messages.Add(new Message { Body = message, IdSender = CurrentUser.Id, IdRecipient = SelectedFriendId, Seen = false, Date = DateTime.Now });
            }
            return null;
        }
        [OnlineUsers.UserAccess]
        public ActionResult Delete(int id)
        {
            DB.Messages.Delete(id);
            return null;
        }
        [OnlineUsers.AdminAccess]
        public ActionResult ViewAllMessages()
        {
            return View();
        }
        [OnlineUsers.AdminAccess(false)]
        public ActionResult GetAllMessages(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Messages.HasChanged)
                return PartialView("AllMessages", DB.Messages.ToList().OrderBy(message => message.Date).Reverse());
            return null;
        }
    }
}