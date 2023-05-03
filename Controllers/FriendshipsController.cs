using ChatManager.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
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
                return PartialView("FriendShips", ApplyFilters(DB.Users.ToList()).OrderBy(user => user.FirstName));
            }
            return null;
        }
        public void Search()
        {

        }
        Func<User, User, bool> FilterNotFriend = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status == null || (status.Denied && status.IsSender(user));
        };

        Func<User, User, bool> FilterRequest = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Pending && status.IsSender(user);
        };

        Func<User, User, bool> FilterPending = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Pending && status.IsSender(currentUser);
        };
        Func<User, User, bool> FilterFriend = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Accepted;
        };
        Func<User, User, bool> FilterRefused = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Denied;
        };


        private List<User> ApplyFilters(IEnumerable<User> users)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            if (!(bool)Session["FilterNotFriend"])
                users = users.Where(user => !FilterNotFriend(user, currentUser));
            if (!(bool)Session["FilterRequest"])
                users = users.Where(user => !FilterRequest(user, currentUser));
            if (!(bool)Session["FilterPending"])
                users = users.Where(user => !FilterPending(user, currentUser));
            if (!(bool)Session["FilterFriend"])
                users = users.Where(user => !FilterFriend(user, currentUser));
            if (!(bool)Session["FilterRefused"])
                users = users.Where(user => !FilterRefused(user, currentUser));
            if (!(bool)Session["FilterBlocked"])
                users = users.Where(user => !user.Blocked);
            return users.ToList();
        }
        public ActionResult SetFilterNotFriend(bool check = false)
        {
            Session["FilterNotFriend"] = check;
            return null;
        }
        public ActionResult SetFilterRequest(bool check = false)
        {
            Session["FilterRequest"] = check;
            return null;
        }
        public ActionResult SetFilterPending(bool check = false)
        {
            Session["FilterPending"] = check;
            return null;
        }
        public ActionResult SetFilterFriend(bool check = false)
        {
            Session["FilterFriend"] = check;
            return null;
        }
        public ActionResult SetFilterRefused(bool check = false)
        {
            Session["FilterRefused"] = check;
            return null;
        }
        public ActionResult SetFilterBlocked(bool check = false)
        {
            Session["FilterBlocked"] = check;
            return null;
        }
    }
}