using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class MessageRepository : Repository<Message>
    {
        public void DeleteWhere(int userId)
        {
            List<Message> list = new List<Message>();
            foreach (Message m in DB.Messages.ToList())
            {
                list.Add(m);
            }
            foreach (Message message in list)
            {
                if (userId == message.IdSender || userId == message.IdRecipient)
                {
                    this.Delete(message.Id);
                }
            }
        }
    }
}