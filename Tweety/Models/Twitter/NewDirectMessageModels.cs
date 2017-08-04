using System;
using System.Collections.Generic;
using System.Text;

namespace Tweety.Models.Twitter
{

    public class NewDirectMessageObject
    {
        public Event @event { get; set; }
    }
      
    public class Event
    {
        public string type { get; set; }
        public NewEvent_MessageCreate message_create { get; set; }
    }

    public class NewEvent_MessageCreate
    {
        public Target target { get; set; }
        public NewEvent_MessageData message_data { get; set; }
    }
 
    public class NewEvent_MessageData
    {
        public string text { get; set; }
    }


}
