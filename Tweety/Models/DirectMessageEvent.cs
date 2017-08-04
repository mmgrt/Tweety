using System;
using System.Collections.Generic;
using System.Text;
using Tweety.Models.Twitter;

namespace Tweety.Models
{
    public class DirectMessageEvent : TwitterEvent
    { 

        public TwitterUser Recipient { get; internal set; }
        public TwitterUser Sender { get; internal set; }

        public string MessageText { get; internal set; }
        public TwitterEntities MessageEntities { get; internal set; }

        public string JsonSource { get; internal set; }
    }
}

