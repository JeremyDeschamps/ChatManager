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
        private bool FilterNotFriend
        {
            get
            {
                if (Session["FilterNotFriend"] == null)
                    Session["FilterNotFriend"] = true;
                return (bool)Session["FilterNotFriend"];
            }
            set
            {
                Session["FilterNotFriend"] = value;
            }
        }
        private bool FilterRequest
        {
            get
            {
                if (Session["FilterRequest"] == null)
                    Session["FilterRequest"] = true;
                return (bool)Session["FilterRequest"];
            }
            set
            {
                Session["FilterRequest"] = value;
            }
        }
        private bool FilterPending
        {
            get
            {
                if (Session["FilterPending"] == null)
                    Session["FilterPending"] = true;
                return (bool)Session["FilterPending"];
            }
            set
            {
                Session["FilterPending"] = value;
            }
        }
        private bool FilterFriend
        {
            get
            {
                if (Session["FilterFriend"] == null)
                    Session["FilterFriend"] = true;
                return (bool)Session["FilterFriend"];
            }
            set
            {
                Session["FilterFriend"] = value;
            }
        }
        private bool FilterRefused
        {
            get
            {
                if (Session["FilterRefused"] == null)
                    Session["FilterRefused"] = true;
                return (bool)Session["FilterRefused"];
            }
            set
            {
                Session["FilterRefused"] = value;
            }
        }
        private bool FilterBlocked
        {
            get
            {
                if (Session["FilterBlocked"] == null)
                    Session["FilterBlocked"] = true;
                return (bool)Session["FilterBlocked"];
            }
            set
            {
                Session["FilterBlocked"] = value;
            }
        }
        private string FilterSearch
        {
            get
            {
                if (Session["FilterSearch"] == null)
                    Session["FilterSearch"] = "";
                return (string)Session["FilterSearch"];
            }
            set
            {
                Session["FilterSearch"] = value;
            }
        }



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
        public ActionResult Search(string text = "")
        {
            FilterSearch = text;
            return null;
        }
        Func<User, User, bool> FilterNotFriendF = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status == null || (status.Denied && status.IsSender(user));
        };

        Func<User, User, bool> FilterRequestF = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Pending && status.IsSender(user);
        };

        Func<User, User, bool> FilterPendingF = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Pending && status.IsSender(currentUser);
        };
        Func<User, User, bool> FilterFriendF = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Accepted;
        };
        Func<User, User, bool> FilterRefusedF = (user, currentUser) =>
        {
            Friendships status = currentUser.StatusWith(user);
            return status != null && status.Denied;
        };


        private List<User> ApplyFilters(IEnumerable<User> users)
        {
            User currentUser = OnlineUsers.GetSessionUser();
            if (!FilterNotFriend)
                users = users.Where(user => !FilterNotFriendF(user, currentUser));
            if (!FilterRequest)
                users = users.Where(user => !FilterRequestF(user, currentUser));
            if (!FilterPending)
                users = users.Where(user => !FilterPendingF(user, currentUser));
            if (!FilterFriend)
                users = users.Where(user => !FilterFriendF(user, currentUser));
            if (!FilterRefused)
                users = users.Where(user => !FilterRefusedF(user, currentUser));
            if (!FilterBlocked)
                users = users.Where(user => !user.Blocked);
            if (FilterSearch != "")
                users = users.Where(user => user.GetFullName().Contains(FilterSearch));
            return users.ToList();
        }
        public ActionResult SetFilterNotFriend(bool check = false)
        {
            FilterNotFriend = check;
            return null;
        }
        public ActionResult SetFilterRequest(bool check = false)
        {
            FilterRequest = check;
            return null;
        }
        public ActionResult SetFilterPending(bool check = false)
        {
            FilterPending = check;
            return null;
        }
        public ActionResult SetFilterFriend(bool check = false)
        {
            FilterFriend = check;
            return null;
        }
        public ActionResult SetFilterRefused(bool check = false)
        {
            FilterRefused = check;
            return null;
        }
        public ActionResult SetFilterBlocked(bool check = false)
        {
            FilterBlocked = check;
            return null;
        }
    }
}