using CorpChat.Common;
using CorpChat.Entities;
using CorpChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorpChat.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            CorpChatUser user = CurrentSession.User;

            if (user != null)
            {
                return user.Username;
            } else
            {
                return "system";
            }
            
            
        }
    }
}