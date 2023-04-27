using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class Friendships
    {
        public Friendships() {
            Pending = true;
            Accepted = false;
            Denied = false;
        }
        public int Id { get; set; }
        public int IdUser1 { get; set; }
        public int IdUser2 { get; set; }
        public int UserSending { get; set; }
        public bool Pending { get; set; } 
        public bool Accepted { get; set; }
        public bool Denied { get; set; }

        [JsonIgnore]
        public User User1 { get => DB.Users.FindUser(IdUser1); }
        
        [JsonIgnore]
        public User User2 { get => DB.Users.FindUser(IdUser2); }

    }
}