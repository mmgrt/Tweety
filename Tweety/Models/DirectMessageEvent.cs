using System;
using System.Collections.Generic;
using System.Text;
using Tweety.Models.Twitter;

namespace Tweety.Models
{
    public class DirectMessageEvent : TwitterEvent
    {

        public TwitterUser Recipient { get; set; }
        public TwitterUser Sender { get; set; }

        public string MessageText { get; set; }
        public TwitterEntities MessageEntities { get; set; }

        public string JsonSource { get; set; }
    }
}

