using System;
using System.Collections.Generic;
using System.Text;

namespace Tweety.Models.Twitter
{
    public class UrlEntity
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public int[] indices { get; set; }
    }
}
