using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweety.Authentication;

namespace Tweety.Tests
{
    public class TestsConstants
    {
        /// <summary>
        /// Auth context to use int tests.
        /// </summary>
        public static TweetyAuthContext AuthContext => new TweetyAuthContext()
        {
            AccessSecret = "",
            AccessToken = "",
            ConsumerKey = "",
            ConsumerSecret = ""
        };


        /// <summary>
        /// Webhook Url to register.
        /// </summary>
        public static string WebhookUrl => "";


        /// <summary>
        /// Webhook Id to test against, Will be set in the RegisterWebhookTest. 
        /// </summary>
        public static string RegsiteredWebhookId { get; set; } = "";
    }
}
