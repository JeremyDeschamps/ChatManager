using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int IdSender { get; set; }

        public int IdRecipient { get; set; }

        public bool Seen { get; set; }
        public string Body { get; set; }


        public DateTime Date { get; set; }

        [JsonIgnore]
        public string StrDate { get => Date.ToString("d MMMM yyyy - H:mm"); }

        [JsonIgnore]
        public User Sender {get => DB.Users.FindUser(IdSender); }

        [JsonIgnore]
        public User Recipient {get => DB.Users.FindUser(IdRecipient); }


        public bool IsSender(User user) => user.Id == idSender;
    }
}