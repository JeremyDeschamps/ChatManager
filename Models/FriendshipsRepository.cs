using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    
    public class FriendshipsRepository : Repository<Friendships>
    {
        public void DeleteWhere(int userId)
        {
            List<Friendships> list = new List<Friendships>();
            foreach (Friendships m in DB.Friendships.ToList())
            {
                list.Add(m);
            }
            foreach (Friendships friendships in list)
            {
                if(userId == friendships.IdUser1 || userId == friendships.IdUser2)
                {
                    this.Delete(friendships.Id);
                }
           }
        }
    }
}