using System;
using System.Collections.Generic;
using System.Text;

namespace Tweety.Models
{
    public class TwitterEvent
    {
        public string Id { get; internal set; }
        public TwitterEventType Type { get; internal set; }
        public DateTime Created { get; internal set; }

    }
}
