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
        public void SetFilterNotFriend(bool check = false)
        {
        }
        public void SetFilterRequest(bool check = false)
        {
        }
        public void SetFilterPending(bool check = false)
        {
        }
        public void SetFilterFriend(bool check = false)
        {
        }
        public void SetFilterRefused(bool check = false)
        {
        }
        public void SetFilterBlocked(bool check = false)
        {
        }
    }
}