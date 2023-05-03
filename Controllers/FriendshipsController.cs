﻿using ChatManager.Models;
using System;
using System.Collections.Generic;
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