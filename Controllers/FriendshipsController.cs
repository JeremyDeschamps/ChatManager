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
        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendFriendRequest(int id) 
        {
            User CurrentUser = OnlineUsers.GetSessionUser();

            if(CurrentUser.StatusWith(CurrentUser.Friends.Find(u => u.Id == id)) != null)
            {
                Friendships friendship = CurrentUser.StatusWith(CurrentUser.Friends.Find(u => u.Id == id));
                DB.Friendships.Delete(friendship.Id);
            }
            
            DB.Friendships.Add(new Friendships() { UserSending = CurrentUser.Id, IdUser1 = CurrentUser.Id, IdUser2 = id });
           
            return RedirectToAction("Index");
        }
        public ActionResult AcceptFriendRequest(int id)
        {
            User CurrentUser = OnlineUsers.GetSessionUser();
            Friendships friendship = CurrentUser.StatusWith(CurrentUser.Friends.Find(u => u.Id == id));
            friendship.Accepted = true;
            friendship.Pending = false;
            DB.Friendships.Update(friendship);
            return RedirectToAction("Index");
        }
        public ActionResult DenyFriendRequest(int id)
        {
            User CurrentUser = OnlineUsers.GetSessionUser();
            Friendships friendship = CurrentUser.StatusWith(CurrentUser.Friends.Find(u => u.Id == id));
            friendship.Denied = true;
            friendship.Pending = false;
            DB.Friendships.Update(friendship);
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFriend(int id)
        {
            User CurrentUser = OnlineUsers.GetSessionUser();
            Friendships friendship = CurrentUser.StatusWith(CurrentUser.Friends.Find(u => u.Id == id));
            DB.Friendships.Delete(friendship.Id);
            return RedirectToAction("Index");
        }
        [OnlineUsers.UserAccess]
        public PartialViewResult GetFriendShipsStatus(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Friendships.HasChanged)
            {
                ViewBag.CurrentUser = OnlineUsers.GetSessionUser();
                return PartialView("FriendShips", DB.Users.ToList().OrderBy(user => user.FirstName));
            }
            return null;
        }
        public void Search()
        {

        }
        public PartialViewResult SetFilterNotFriend(bool check = false)
        {
           
            return null;
        }
        public PartialViewResult SetFilterRequest(bool check = false)
        {
            
            return null;
        }
        public PartialViewResult SetFilterPending(bool check = false)
        {
           
            return null;
        }
        public PartialViewResult SetFilterFriend(bool check = false)
        {
            return null;
        }
        public PartialViewResult SetFilterRefused(bool check = false)
        {
            
            return null;
        }
        public PartialViewResult SetFilterBlocked(bool check = false)
        {
            return null;
        }
    }
}